import tkinter as tk
from tkinter import ttk, messagebox
from sqlalchemy import text
from database import get_session, Grant, Scientist, Direction
from datetime import datetime

# КЛАСС КАРТОЧКИ (Аналог UserControlOne.xaml)
class GrantCard(tk.Frame):
    def __init__(self, parent, grant_data):
        super().__init__(parent, relief=tk.RAISED, borderwidth=1, bg="white")
        self.pack(fill=tk.X, padx=10, pady=5)

        # Левая часть: Иконка (как в XAML)
        icon_frame = tk.Frame(self, width=80, height=80, bg="#E0E0E0")
        icon_frame.pack(side=tk.LEFT, padx=10, pady=10)
        icon_frame.pack_propagate(False)
        tk.Label(icon_frame, text="📄", font=("Arial", 30), bg="#E0E0E0", fg="gray").place(relx=0.5, rely=0.5, anchor=tk.CENTER)

        # Центральная часть: Информация
        info_frame = tk.Frame(self, bg="white")
        info_frame.pack(side=tk.LEFT, fill=tk.BOTH, expand=True, padx=10)

        tk.Label(info_frame, text=grant_data['name'], font=("Arial", 12, "bold"), bg="white", wraplength=400, justify=tk.LEFT).pack(anchor=tk.W)
        tk.Label(info_frame, text=f"Ученый: {grant_data['fio']}", bg="white").pack(anchor=tk.W)
        tk.Label(info_frame, text=f"Направление: {grant_data['dir']}", bg="white").pack(anchor=tk.W)
        
        date_str = f"Срок: {grant_data['start']} — {grant_data['end']}"
        tk.Label(info_frame, text=date_str, bg="white", fg="gray").pack(anchor=tk.W)
        tk.Label(info_frame, text=f"Организация: {grant_data['org']}", bg="white").pack(anchor=tk.W)

        # Правая часть: Сумма
        sum_frame = tk.Frame(self, bg="white")
        sum_frame.pack(side=tk.RIGHT, padx=20)
        tk.Label(sum_frame, text=f"{grant_data['sum']:,.0f} руб.".replace(',', ' '), font=("Arial", 13, "bold"), bg="white", fg="#2E7D32").pack()

# ОСНОВНОЕ ОКНО
class GrantApp:
    def __init__(self, root):
        self.root = root
        self.root.title("Научный фонд - Карточки грантов")
        self.root.geometry("8500x600")
        
        self.db_type = "mysql"
        self.mode = "linq"
        self.db = get_session(self.db_type)

        self.setup_ui()
        self.load_combos()
        self.refresh_data()

    def setup_ui(self):
        # Верхняя панель (Кнопки)
        top_panel = tk.Frame(self.root, height=50)
        top_panel.pack(fill=tk.X)

        tk.Button(top_panel, text="LINQ", width=15, command=lambda: self.set_mode("linq")).pack(side=tk.LEFT, padx=10, pady=10)
        tk.Button(top_panel, text="SQL", width=15, command=lambda: self.set_mode("sql")).pack(side=tk.LEFT, padx=5, pady=10)
        
        tk.Label(top_panel, text="|").pack(side=tk.LEFT, padx=10)
        
        tk.Button(top_panel, text="MySQL", bg="#e1f5fe", command=lambda: self.switch_db("mysql")).pack(side=tk.LEFT, padx=5)
        tk.Button(top_panel, text="PostgreSQL", bg="#ede7f6", command=lambda: self.switch_db("postgre")).pack(side=tk.LEFT, padx=5)

        # Панель фильтров
        filter_panel = tk.Frame(self.root)
        filter_panel.pack(fill=tk.X, padx=10)

        tk.Label(filter_panel, text="Ученый:").pack(side=tk.LEFT)
        self.fio_cb = ttk.Combobox(filter_panel, width=30)
        self.fio_cb.pack(side=tk.LEFT, padx=10, pady=5)
        self.fio_cb.bind("<<ComboboxSelected>>", lambda e: self.refresh_data())

        tk.Label(filter_panel, text="Направление:").pack(side=tk.LEFT)
        self.dir_cb = ttk.Combobox(filter_panel, width=30)
        self.dir_cb.pack(side=tk.LEFT, padx=10, pady=5)
        self.dir_cb.bind("<<ComboboxSelected>>", lambda e: self.refresh_data())

        # ОБЛАСТЬ СКРОЛЛА (Аналог ListBox + ScrollViewer)
        self.canvas = tk.Canvas(self.root, bg="#F5F5F5")
        self.scrollbar = ttk.Scrollbar(self.root, orient="vertical", command=self.canvas.yview)
        self.scrollable_frame = tk.Frame(self.canvas, bg="#F5F5F5")

        self.scrollable_frame.bind(
            "<Configure>",
            lambda e: self.canvas.configure(scrollregion=self.canvas.bbox("all"))
        )

        self.canvas.create_window((0, 0), window=self.scrollable_frame, anchor="nw", width=800)
        self.canvas.configure(yscrollcommand=self.scrollbar.set)

        self.canvas.pack(side="left", fill="both", expand=True)
        self.scrollbar.pack(side="right", fill="y")

    def switch_db(self, db_type):
        self.db.close()
        self.db_type = db_type
        self.db = get_session(db_type)
        self.load_combos()
        self.refresh_data()
        messagebox.showinfo("Инфо", f"Переключено на {db_type.upper()}")

    def set_mode(self, mode):
        self.mode = mode
        self.refresh_data()

    def load_combos(self):
        scients = ["Все"] + [s[0] for s in self.db.query(Scientist.fio_scient).all()]
        dirs = ["Все"] + [d[0] for d in self.db.query(Direction.name_direction).all()]
        self.fio_cb['values'] = scients
        self.dir_cb['values'] = dirs
        self.fio_cb.current(0)
        self.dir_cb.current(0)

    def refresh_data(self):
        # Очистка старых карточек
        for widget in self.scrollable_frame.winfo_children():
            widget.destroy()

        sel_fio = self.fio_cb.get()
        sel_dir = self.dir_cb.get()

        data_list = []

        if self.mode == "linq":
            # Логика LINQ (ORM)
            query = self.db.query(Grant).join(Scientist).join(Direction)
            if sel_fio != "Все":
                query = query.filter(Scientist.fio_scient == sel_fio)
            if sel_dir != "Все":
                query = query.filter(Direction.name_direction == sel_dir)
            
            for g in query.all():
                data_list.append({
                    'name': g.name_theme,
                    'fio': g.scientist.fio_scient,
                    'dir': g.direction.name_direction,
                    'start': g.date_start.strftime("%d.%m.%Y") if g.date_start else "-",
                    'end': g.date_end.strftime("%d.%m.%Y") if g.date_end else "-",
                    'org': g.organization,
                    'sum': g.summa or 0
                })
        else:
            # Логика SQL (Raw SQL)
            sql = """
                SELECT g.name_theme, s.fio_scient, d.name_direction, 
                       g.date_start, g.date_end, g.organization, g.summa
                FROM grants g
                JOIN scientists s ON g.id_scient = s.id_scient
                JOIN directions d ON g.id_direction = d.id_direction
                WHERE 1=1
            """
            params = {}
            if sel_fio != "Все":
                sql += " AND s.fio_scient = :fio"
                params['fio'] = sel_fio
            if sel_dir != "Все":
                sql += " AND d.name_direction = :dir"
                params['dir'] = sel_dir
            
            result = self.db.execute(text(sql), params)
            for row in result:
                data_list.append({
                    'name': row[0], 'fio': row[1], 'dir': row[2],
                    'start': row[3].strftime("%d.%m.%Y") if row[3] else "-",
                    'end': row[4].strftime("%d.%m.%Y") if row[4] else "-",
                    'org': row[5], 'sum': row[6] or 0
                })

        # Создание карточек
        for item in data_list:
            GrantCard(self.scrollable_frame, item)

if __name__ == "__main__":
    root = tk.Tk()
    app = GrantApp(root)
    root.mainloop()