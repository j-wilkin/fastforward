select * from videos

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_s67ekkgw', 'First Choice College', 'Learn why a lot of students do not get into their first choice of college and what you can do to improve your chances of getting into yours.', 1
where not exists (
	select *
	from Videos
	where VideoId = '0_s67ekkgw' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_q2ndlts6', 'Internet Warning', 'Hear from students why it''s a good idea to closely monitor what you post online and who can see it.', 2
where not exists (
	select *
	from Videos
	where VideoId = '0_q2ndlts6' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_7j2yiknd', 'Time Management', 'Time management is tricky, especially at college. Hear from current students how time management affected their academics and how they adapted.', 3
where not exists (
	select *
	from Videos
	where VideoId = '0_7j2yiknd' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_wwy31sn9', 'Writing', 'Find out how different writing in high school is versus writing in college.', 4
where not exists (
	select *
	from Videos
	where VideoId = '0_wwy31sn9' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_w4kwwri3', 'Research', 'Learn how to use effectively use your resources to write a great college research paper.', 5
where not exists (
	select *
	from Videos
	where VideoId = '0_w4kwwri3' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_qrl2ye0a', 'Memory', 'Note taking is essential in any college classroom. Some students discuss what they found difficult to memorize in college classes.', 6
where not exists (
	select *
	from Videos
	where VideoId = '0_qrl2ye0a' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_urpfc4nd', 'Lecture', 'Learn how to make the most and learn effectively in a large college course where it''s easy to go unnoticed.', 7
where not exists (
	select *
	from Videos
	where VideoId = '0_urpfc4nd' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_sykn1qpd', 'Job Fairs', 'Students talk about their difference experience at job fairs, and whether they are worth your time or not.', 8
where not exists (
	select *
	from Videos
	where VideoId = '0_sykn1qpd' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_ql9dmwf5', 'Networking', 'Networking is one of the best methods of career advancement and getting to know other professionals and mentors. Hear some stories about networking and how resourceful it is.', 9
where not exists (
	select *
	from Videos
	where VideoId = '0_ql9dmwf5' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_l0u78r5c', 'Interview ', 'Learn more about job interviews and the different ways to approach them.', 10
where not exists (
	select *
	from Videos
	where VideoId = '0_l0u78r5c' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_f4nlobzm', 'Apprenticeship', 'Hear about how one student got their first apprenticeship.', 11
where not exists (
	select *
	from Videos
	where VideoId = '0_f4nlobzm' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_g29a91ch', 'Managing', 'Learn how to manage doing what you love and why leadership skills are essential to any occupation.', 12
where not exists (
	select *
	from Videos
	where VideoId = '0_g29a91ch' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_xpf4696t', 'Worst Job', 'Many students go into an occupation and come out hating it. What did these students do and why was it the worst job they''d ever had?', 13
where not exists (
	select *
	from Videos
	where VideoId = '0_xpf4696t' )

insert into Videos (VideoId, Name, [Description], DisplayOrder)
select '0_mumicioe', 'Degree & Jobs', 'Current students talk about their majors and the opportunities that lie within and beyond their academic focuses.', 14
where not exists (
	select *
	from Videos
	where VideoId = '0_mumicioe' )

