------------------------------------------------------------------------
--                          C U R S O R S
------------------------------------------------------------------------
/*

 5 STEPS IN CURSOR:     (it's like a pointer in c programming na?)
 Declare → Open → Fetch → Process → Close

*/

do $$
declare
    rental_record record;
    rental_cursor cursor for
        select r.rental_id, c.first_name, c.last_name, r.rental_date
        from rental r
        join customer c on r.customer_id = c.customer_id
        order by r.rental_id;
begin
    open rental_cursor;

    loop
        fetch rental_cursor into rental_record;
        exit when not found;

        raise notice 'rental id: %, customer: % %, date: %',
                     rental_record.rental_id,
                     rental_record.first_name,
                     rental_record.last_name,
                     rental_record.rental_date;
    end loop;

    close rental_cursor;
end;
$$;

-------------------------------------example 2----------------------

create table rental_tax_log (
    rental_id int,
    customer_name text,
    rental_date timestamp,
    amount numeric,
    tax numeric
);

select * from rental_tax_log
do $$
declare
    rec record;
    cur cursor for
        select r.rental_id, 
               c.first_name || ' ' || c.last_name as customer_name,
               r.rental_date,
               p.amount
        from rental r
        join payment p on r.rental_id = p.rental_id
        join customer c on r.customer_id = c.customer_id;
begin
    open cur;

    loop
        fetch cur into rec;
        exit when not found;

        -- cursor can be used to insert into tables too!
        insert into rental_tax_log (rental_id, customer_name, rental_date, amount, tax)
        values (
            rec.rental_id,
            rec.customer_name,
            rec.rental_date,
            rec.amount,
            rec.amount * 0.10
        );
    end loop;

    close cur;
end;
$$;