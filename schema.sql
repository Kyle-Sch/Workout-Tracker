
-- Switch to the system (aka master) database
USE master;
GO

-- Delete the WorkoutDB Database (IF EXISTS)
--IF EXISTS(select * from sys.databases where name='WorkoutDB')
--drop table workout;
--drop table visit;
--drop table equipment;
--drop table users;
--DROP DATABASE WorkoutDB;
--GO

-- Create a new WorkoutDB Database
CREATE DATABASE WorkoutDB;
GO

-- Switch to the WorkoutDB Database
USE WorkoutDB
GO

BEGIN TRANSACTION;


CREATE TABLE users 
(
	id				int				identity(1,1),
	username		varchar(50)		not null,
	firstname		varchar(50)     null,
	lastname		varchar(50)		null,
	password		varchar(50)		not null,
	salt			varchar(50)		not null,
	role			varchar(50)		default('member'),
	photo			varchar(50)		null,
	message			varchar(50)		null,
	goalType		varchar(50)		null,
	goalReps		decimal		 	null,
	isActive		bit				not null,
	
	constraint pk_users primary key (id)

);

CREATE TABLE equipment
(
	equipmentId			int			identity(1,1),
	name				varchar(50)	not null,
	category			varchar(50) not null,
	needsMaintenance	bit			not null,
	formMedia			varchar(MAX) not null,
	instructions		varchar(MAX) not null,
	isActive			bit			not null,

	constraint pk_equipment primary key (equipmentId)
	
);

CREATE TABLE visit
(
	visitId				int			identity(1,1),
	memberId			int			not null,
	arrival				datetime	not null,
	departure			datetime	null,
	isActive			bit			not null,

	constraint pk_visit primary key (visitId),
	constraint fk_users foreign key (memberId) REFERENCES users(id)
);

CREATE TABLE workout
(
	workoutId			int			identity(1,1),
	visitId				int			not null,
	name				varchar(50) not null,
	type				varchar(50)	not null,
	reps				decimal 	null,
	weight				decimal		null,
	startTime			datetime	null,
	endTime				datetime	null,
	equipmentId			int			not null,
	userId				int			not null,
	isActive			bit			not null,

	constraint pk_workout primary key (workoutId),
	constraint fk_visit foreign key (visitId) REFERENCES visit(visitId),
	constraint fk_equipment foreign key (equipmentId) REFERENCES equipment(equipmentId),
	constraint fk_User foreign key (userId) REFERENCES users(Id)

);

CREATE TABLE workoutClass
(
	classId				int			identity(1,1),
	name				varchar(50) not null,
	instructorName		varchar(50) null,
	availableSpots		int			null,
	description			varchar(50)	null,
	startTime			datetime	null,
	endTime				datetime	null,
	isActive			bit			not null,

);

INSERT INTO users(username, password, salt, role, isActive) VALUES ('nico.cersosimo.22@gmail.com', '+Er1hpQ6dSYHBr/5pCATAWRYZrw=', 'cf5lC7Z2BwI=', 'admin', 'true');
INSERT INTO users(username, password, salt, role, isActive) VALUES ('kyle@gmail.com', '/+0ZwkaXvhuf3PmYCXWb3NR2GvE=', 'yxlgbG7QHVU=', 'admin', 'true')
INSERT INTO users(username, password, salt, role, isActive) VALUES ('corey@gmail.com', 'Yc4ZnhSE2P+JlWh+4I+QgAL4DkA=', 'xkvCNtNX87c=', 'admin', 'true');
INSERT INTO users(username, password, salt, role, isActive) VALUES ('yoyo@capstone.com', '4tDtPj2aDMrEzcvyMLng0EOSjEY=', 'bCfx1/0hs1M=', 'admin', 'true');
INSERT INTO visit(memberId, isActive, arrival, departure) VALUES (3, 'true', '2018-12-15 12:00:00', '2018-12-16 00:00:00');
INSERT INTO visit(memberId, isActive, arrival, departure) VALUES (3, 'true', '2018-12-15 12:00:00', '2018-12-15 12:30:00');
INSERT INTO equipment VALUES ('Treadmill','Cardio','false','treadmill.jpg','Start the treadmill at a walking speed. Warmup with that speed or a slight-jogging pace for at least a minute, then begin your desired pace and time. While running, swing your arms rather than holding on to the arms to maintain proper running form. When finished, retract to the same speed you warmed up with for at least a minute or longer cooldown period.','true');
INSERT INTO equipment VALUES ('Elliptical','Cardio','false','elliptical.jpg','Step onto the machine facing the monitor. Grab hold of the arms and begin pedaling to turn the machine on. Begin at little to no resistance for at least a minute to get warmed up, and then increase to desired resistance for your workout. Retract to the same amount of resistance as your warmup for the cooldown when finished, also for at least a minute. Never let go of arm handles while pedaling','true');
INSERT INTO equipment VALUES ('Bench Press','Upper Body','false','benchpress.jpg','Place even amount of weight on each side of the bar, then secure them with clamps. Lay your back on the bench with your head towards the bar, and vertically line your neck up to about where the bar is (can adjust slightly for your own comfort, but this is generally the best placement form-wise). Always have a spotter standing directly behind the bar with their hands ready to assist you, if necessary. Lift bar straight up above its holder, and then out horizontally, directly above your chest (if it is a free-bar bench press, if it is a more guided machine, you usually have to lift the bar and rotate it slightly forward to release it from its holder). Lower the bar down until it just knicks your chest, but not to the point where your chest is supporting any of the weight. Then, lift the bar back up to the highest point BEFORE your elbows lock. Repeat this process until you are done with your reps, then notify your spotter that you are done. Allow them to assist you in placing the bar back into its holder.','true');
INSERT INTO equipment VALUES ('Hand Weights','Upper Body','false','handweights.jpg','Grab the weight directly in the middle of its bar, and maintain a firm grip. Consistently slow reps will do more for you than flying through it (as with any upper-body workout).','true');
INSERT INTO equipment VALUES ('Leg Press','Lower Body','false','legpress.jpg','Place weights on bar just as you would with the bench press, and clamp them. Sit with back straight on the seat and not slouching. Place feet directly in the middle of the press about shoulder length apart. Push weight past its holders and then turn the handles of the holders outward to release them. Grab on to the seat handles, and bring legs as close to your chest as possible without resting the weight on it. Then, push back out to the farthest point BEFORE locking your knees. Repeat the process until you finish your reps. On the last outward push, turn holders back inward and rest the press on them.','true');
INSERT INTO equipment VALUES ('None','None','false','nothing.jpg','None Required.','true');
INSERT INTO workoutClass VALUES ('Yoga','Mr. WorkHard',20,'Improve Flexability.','2018-12-25 10:00:00','2018-12-25 11:00:00','true');
INSERT INTO workoutClass VALUES ('Group Biking','Ms. WorkHard',15,'Bike in a group for increased motivation.','2018-12-26 10:00:00','2018-12-26 11:00:00','true');
INSERT INTO workoutClass VALUES ('Water Aerobics','Mr. PlayHard',30,'Minor stress workout.','2019-1-7 10:00:00','2019-1-7 11:00:00','true');
INSERT INTO workoutClass VALUES ('Kick Boxing','Ms. PlayHard',25,'Kick out those calories!','2019-1-15 10:00:00','2019-1-15 11:00:00','true');
COMMIT TRANSACTION;