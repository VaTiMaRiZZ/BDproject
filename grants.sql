create table grants
(
    id_grant       integer      not null
        primary key,
    direction_code varchar(8)
        references directions (),
    manager_id     integer
        references scientists,
    topic          varchar(100) not null,
    start_date     date         not null,
    organization   varchar(60)  not null,
    end_date       date         not null,
    amount         numeric(10)  not null
        constraint grants_amount_check
            check (amount > (0)::numeric),
    constraint grants_check
        check (end_date > start_date)
);

alter table grants
    owner to postgres;

INSERT INTO public.grants (id_grant, direction_code, manager_id, topic, start_date, organization, end_date, amount) VALUES (1001, 'IT', 2, 'Разработка ИИ', '2023-01-01', 'МГПУ', '2023-01-18', 100000);
INSERT INTO public.grants (id_grant, direction_code, manager_id, topic, start_date, organization, end_date, amount) VALUES (1002, 'BIO', 1, 'Геометрия', '2023-07-21', 'МГПУ', '2023-07-31', 50000);
