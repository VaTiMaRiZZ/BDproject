create table scientists
(
    id_scient     integer not null
        primary key,
    fio_scient    varchar(100),
    birth_date    date,
    degree_scient varchar(50),
    title         varchar(50)
);

alter table scientists
    owner to postgres;

INSERT INTO public.scientists (id_scient, fio_scient, birth_date, degree_scient, title) VALUES (1, 'Иванов И.И.', '1980-05-12', 'Кандидат наук', 'Доцент');
INSERT INTO public.scientists (id_scient, fio_scient, birth_date, degree_scient, title) VALUES (2, 'Петров П.П.', '1975-03-20', 'Доктор наук', 'Профессор');
INSERT INTO public.scientists (id_scient, fio_scient, birth_date, degree_scient, title) VALUES (3, 'Сидорова С.С.', '1990-08-15', 'Нет', 'Младший сотрудник');
INSERT INTO public.scientists (id_scient, fio_scient, birth_date, degree_scient, title) VALUES (4, 'Кузнецов К.К.', '1985-11-01', 'Кандидат наук', 'Доцент');
