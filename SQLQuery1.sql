create database avaliacaoDesenvolvedorDotNet
go
use avaliacaoDesenvolvedorDotNet
go
create table contact
(
	id int not null identity(1,1),
	name varchar(50)

	primary key (id)
);
go
create table contactNumber
(
	id int not null identity(1,1),
	idContact int not null,
	number varchar(20)
	primary key (id),
	foreign key (idContact) references contact(id)
);