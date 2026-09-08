create table participants
(
    grant_id     integer not null
        constraint participants_grants_id_grant_fk
            references grants,
    scientist_id integer not null
        constraint participants_scientists_id_scient_fk
            references scientists,
    primary key (grant_id, scientist_id)
);

alter table participants
    owner to postgres;

INSERT INTO public.participants (grant_id, scientist_id) VALUES (1001, 1);
INSERT INTO public.participants (grant_id, scientist_id) VALUES (1002, 4);
