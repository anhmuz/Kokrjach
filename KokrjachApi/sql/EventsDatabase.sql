use kokrjach_events;

drop table if exists event_item;

create table event_item (
	id int not null auto_increment,
    user_id varchar(255) not null,
    event_description varchar(255),
    primary key (id)
);
