import tkinter as tk
from tkinter import ttk, messagebox
from sqlalchemy import text
from database import get_session, Grant, Scientist, Direction
from datetime import datetime

class GrantApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Управление грантами (Multi-DB)")
        self.edit_grant_id = None
        self.current_mode = "linq" 
        self.current_db_type = "mysql"
        self.db = get_session(self.current_db_type)
        self.setup_ui()
        self.load_combo_boxes()
        self.load_data_to_grid()
        
    def setup_ui(self):
        top_frame = tk.Frame(self.root)
        top_frame.pack(fill=tk.X, padx=5, pady=5)
        tk.Label(top_frame, text="Режим:").pack(side=tk.LEFT)
        tk.Button(top_frame, text="Linq", command=self.switch_to_linq, bg="lightgray").pack(side=tk.LEFT, padx=2)
        tk.Button(top_frame, text="SQL", command=self.switch_to_sql, bg="lightgray").pack(side=tk.LEFT, padx=2)
        tk.Label(top_frame, text=" | БД:").pack(side=tk.LEFT, padx=(10,0))
        tk.Button(top_frame, text="MySQL", command=lambda: self.switch_db("mysql")).pack(side=tk.LEFT, padx=2)
        tk.Button(top_frame, text="PostgreSQL", command=lambda: self.switch_db("postgre")).pack(side=tk.LEFT, padx=2)
        tk.Button(top_frame, text="Выход", command=self.exit_app, fg="red").pack(side=tk.RIGHT, padx=2)
        self.status_label = tk.Label(self.root, text=f"Текущая БД: {self.current_db_type} | Режим: {self.current_mode}", bd=1, relief=tk.SUNKEN, anchor=tk.W)
        self.status_label.pack(side=tk.BOTTOM, fill=tk.X)

        filter_frame = tk.Frame(self.root)
        filter_frame.pack(fill=tk.X, padx=5, pady=5)
        tk.Label(filter_frame, text="Ученый:").pack(side=tk.LEFT)
        self.combo_scient = ttk.Combobox(filter_frame, width=30)
        self.combo_scient.pack(side=tk.LEFT, padx=5)
        self.combo_scient.bind("<<ComboboxSelected>>", self.on_filter_change)
        
        tk.Label(filter_frame, text="Направление:").pack(side=tk.LEFT, padx=(10,0))
        self.combo_direction = ttk.Combobox(filter_frame, width=30)
        self.combo_direction.pack(side=tk.LEFT, padx=5)
        self.combo_direction.bind("<<ComboboxSelected>>", self.on_filter_change)

        self.tree = ttk.Treeview(self.root, columns=("ID", "Grant", "Start", "End", "Org", "Sum", "Scient", "Dir"), show="headings")
        headings = [("ID", 40), ("Grant", 150), ("Start", 100), ("End", 100), ("Org", 120), ("Sum", 80), ("Scient", 150), ("Dir", 150)]
        for col, width in headings:
            self.tree.heading(col, text=col)
            self.tree.column(col, width=width)
        scrollbar = ttk.Scrollbar(self.root, orient=tk.VERTICAL, command=self.tree.yview)
        self.tree.configure(yscrollcommand=scrollbar.set)
        self.tree.pack(fill=tk.BOTH, expand=True, padx=5, pady=5)
        scrollbar.pack(side=tk.RIGHT, fill=tk.Y)
        self.tree.bind("<<TreeviewSelect>>", self.on_tree_select)
        
        edit_frame = tk.Frame(self.root)
        edit_frame.pack(fill=tk.X, padx=5, pady=5)
        fields = [
            ("Название:", "entry_name", 0, 0),
            ("Дата начала (YYYY-MM-DD):", "entry_start", 0, 2),
            ("Дата конца:", "entry_end", 0, 4),
            ("Организация:", "entry_org", 1, 0),
            ("Сумма:", "entry_sum", 1, 2)
        ]
        for label_text, attr_name, r, c in fields:
            tk.Label(edit_frame, text=label_text).grid(row=r, column=c, sticky="e")
            entry = tk.Entry(edit_frame, width=20)
            entry.grid(row=r, column=c+1, padx=5, pady=2)
            setattr(self, attr_name, entry)
        tk.Button(edit_frame, text="Сохранить", command=self.save_grant, bg="lightgreen").grid(row=1, column=4, padx=10)
        tk.Button(edit_frame, text="Удалить", command=self.delete_grant, bg="#ffcccb").grid(row=1, column=5)

    def switch_db(self, db_type):
        try:
            self.db.close()
            self.db = get_session(db_type)
            self.current_db_type = db_type
            self.load_combo_boxes()
            self.load_data_to_grid()
            self.update_status()
            messagebox.showinfo("БД", f"Переключено на {db_type}")
        except Exception as e:
            messagebox.showerror("Ошибка подключения", f"Не удалось подключиться к {db_type}: {e}")

    def update_status(self):
        self.status_label.config(text=f"Текущая БД: {self.current_db_type} | Режим: {self.current_mode}")

    def load_combo_boxes(self):
        try:
            scientists = self.db.query(Scientist.fio_scient).all()
            self.combo_scient['values'] = ["Все"] + [s[0] for s in scientists]
            self.combo_scient.current(0)
            
            directions = self.db.query(Direction.name_direction).all()
            self.combo_direction['values'] = ["Все"] + [d[0] for d in directions]
            self.combo_direction.current(0)
        except Exception as e:
            messagebox.showerror("Ошибка", f"Ошибка загрузки справочников: {e}")

    def load_data_to_grid(self):
        for item in self.tree.get_children():
            self.tree.delete(item)
            
        selected_scient = self.combo_scient.get()
        selected_dir = self.combo_direction.get()
        
        try:
            if self.current_mode == "linq":
                query = self.db.query(Grant).join(Scientist).join(Direction)
                if selected_scient != "Все":
                    query = query.filter(Scientist.fio_scient.contains(selected_scient))
                if selected_dir != "Все":
                    query = query.filter(Direction.name_direction.contains(selected_dir))
                    
                results = query.all()
                for g in results: # ИСПРАВЛЕНО: Теперь инсерт внутри цикла
                    start_date = g.date_start.strftime("%Y-%m-%d") if g.date_start else ""
                    end_date = g.date_end.strftime("%Y-%m-%d") if g.date_end else ""
                    self.tree.insert("", tk.END, values=(
                        g.id_grants, g.name_theme, start_date, end_date, 
                        g.organization, g.summa, g.scientist.fio_scient, g.direction.name_direction
                    ))
            else: 
                sql = """
                    SELECT g.id_grants, g.name_theme, g.date_start, g.date_end, 
                        g.organization, g.summa, s.fio_scient, d.name_direction
                    FROM grants g
                    JOIN scientists s ON g.id_scient = s.id_scient
                    JOIN directions d ON g.id_direction = d.id_direction
                    WHERE 1=1
                """
                params = {}
                if selected_scient != "Все":
                    sql += " AND s.fio_scient LIKE :scient"
                    params['scient'] = f"%{selected_scient}%"
                if selected_dir != "Все":
                    sql += " AND d.name_direction LIKE :dir"
                    params['dir'] = f"%{selected_dir}%"
                    
                result = self.db.execute(text(sql), params)
                for row in result:
                    start_date = row[2].strftime("%Y-%m-%d") if row[2] else ""
                    end_date = row[3].strftime("%Y-%m-%d") if row[3] else ""
                    self.tree.insert("", tk.END, values=(
                        row[0], row[1], start_date, end_date, row[4], row[5], row[6], row[7]
                    ))
        except Exception as e:
            messagebox.showerror("Ошибка данных", f"Не удалось загрузить данные: {e}")

    def save_grant(self):
        try:
            name = self.entry_name.get()
            scientist_name = self.combo_scient.get()
            direction_name = self.combo_direction.get()
            
            if not name or scientist_name == "Все" or direction_name == "Все":
                messagebox.showerror("Ошибка", "Заполните название и выберите конкретного ученого/направление")
                return
                
            scientist_obj = self.db.query(Scientist).filter(Scientist.fio_scient == scientist_name).first()
            direction_obj = self.db.query(Direction).filter(Direction.name_direction == direction_name).first()
            
            def parse_date(date_str):
                return datetime.strptime(date_str, "%Y-%m-%d") if date_str.strip() else None

            start_date = parse_date(self.entry_start.get())
            end_date = parse_date(self.entry_end.get())
            summa = int(self.entry_sum.get()) if self.entry_sum.get() else 0
            
            if self.edit_grant_id:
                grant = self.db.query(Grant).filter(Grant.id_grants == self.edit_grant_id).first()
                if grant:
                    grant.name_theme = name
                    grant.date_start = start_date
                    grant.date_end = end_date
                    grant.organization = self.entry_org.get()
                    grant.summa = summa
                    grant.id_scient = scientist_obj.id_scient
                    grant.id_direction = direction_obj.id_direction
            else: 
                new_grant = Grant(
                    name_theme=name, date_start=start_date, date_end=end_date,
                    organization=self.entry_org.get(), summa=summa,
                    id_scient=scientist_obj.id_scient, id_direction=direction_obj.id_direction
                )
                self.db.add(new_grant)
                
            self.db.commit()
            self.clear_form()
            self.load_data_to_grid()
            messagebox.showinfo("Успех", "Данные сохранены")
        except ValueError:
            messagebox.showerror("Ошибка", "Неверный формат даты (ГГГГ-ММ-ДД) или суммы")
        except Exception as e:
            self.db.rollback()
            messagebox.showerror("Ошибка БД", str(e))

    def delete_grant(self):
        selected = self.tree.selection()
        if not selected: return
            
        grant_id = self.tree.item(selected[0])['values'][0]
        try:
            grant_to_delete = self.db.query(Grant).filter(Grant.id_grants == grant_id).first()
            if grant_to_delete:
                self.db.delete(grant_to_delete)
                self.db.commit()
                self.load_data_to_grid()
                self.clear_form()
        except Exception as e:
            messagebox.showerror("Ошибка", str(e))

    def on_tree_select(self, event):
        selected = self.tree.selection()
        if not selected: return
        
        v = self.tree.item(selected[0])['values']
        self.edit_grant_id = v[0]
        self.entry_name.delete(0, tk.END); self.entry_name.insert(0, v[1])
        self.entry_start.delete(0, tk.END); self.entry_start.insert(0, v[2])
        self.entry_end.delete(0, tk.END); self.entry_end.insert(0, v[3])
        self.entry_org.delete(0, tk.END); self.entry_org.insert(0, v[4])
        self.entry_sum.delete(0, tk.END); self.entry_sum.insert(0, v[5])
        self.combo_scient.set(v[6])
        self.combo_direction.set(v[7])

    def clear_form(self):
        self.edit_grant_id = None
        for attr in ["entry_name", "entry_start", "entry_end", "entry_org", "entry_sum"]:
            getattr(self, attr).delete(0, tk.END)

    def switch_to_linq(self):
        self.current_mode = "linq"
        self.update_status()
        self.load_data_to_grid()
        
    def switch_to_sql(self):
        self.current_mode = "sql"
        self.update_status()
        self.load_data_to_grid()

    def on_filter_change(self, event):
        self.load_data_to_grid()

    def exit_app(self):
        self.db.close()
        self.root.destroy()

if __name__ == "__main__":
    root = tk.Tk()
    app = GrantApp(root)
    root.mainloop()