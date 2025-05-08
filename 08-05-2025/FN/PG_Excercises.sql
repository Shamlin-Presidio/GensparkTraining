-- I N T E R M E D I A T E     C A T E G O R Y
-- J O I N S  


SELECT bks.starttime 
FROM 
cd.bookings bks INNER JOIN cd.members mems
ON mems.memid = bks.memid
WHERE mems.firstname='David' AND mems.surname='Farrell';   


SELECT bks.starttime as start, facs.name as name
FROM cd.facilities facs JOIN cd.bookings bks
ON facs.facid = bks.facid
WHERE facs.name like 'Tennis%' AND bks.starttime >= '2012-09-21' AND bks.starttime < '2012-09-22'
ORDER BY bks.starttime;          



SELECT DISTINCT rec.firstname as firstname, rec.surname as surname
FROM cd.members mem JOIN cd.members rec
ON rec.memid = mem.recommendedby
ORDER BY surname, firstname;    

-- ALTERNATIVE APPROACH< WITHOUT JOIN:

SELECT DISTINCT firstname, surname
FROM cd.members
WHERE memid IN (
  SELECT recommendedby
  FROM cd.members
  WHERE recommendedby IS NOT NULL
)
ORDER BY surname, firstname;



SELECT mems.firstname memfname, mems.surname memsname, recs.firstname recfname, recs.surname recsname
FROM cd.members mems LEFT OUTER JOIN cd.members recs
ON recs.memid = mems.recommendedby
ORDER BY memsname, memfname;   


SELECT DISTINCT CONCAT(mems.firstname, ' ', mems.surname) AS member, facs.name AS facility
FROM cd.members mems
JOIN cd.bookings bks ON mems.memid = bks.memid
JOIN cd.facilities facs ON bks.facid = facs.facid
WHERE facs.name LIKE 'Tennis%'
ORDER BY member, facility;



----------------------------------------------------------------------------------------------------
-- 									08-05-2025 
----------------------------------------------------------------------------------------------------
 

SELECT 
  CONCAT(mems.firstname,' ', mems.surname) AS member, 
  facs.name AS facility, 
  CASE 
    WHEN mems.memid = 0 THEN bks.slots * facs.guestcost
    ELSE bks.slots * facs.membercost
  END AS cost
FROM
  cd.members mems
  INNER JOIN cd.bookings bks ON mems.memid = bks.memid
  INNER JOIN cd.facilities facs ON bks.facid = facs.facid
WHERE
  bks.starttime >= '2012-09-14' 
  AND bks.starttime < '2012-09-15' 
  AND (
    (mems.memid = 0 AND bks.slots * facs.guestcost > 30) OR
    (mems.memid != 0 AND bks.slots * facs.membercost > 30)
  )
ORDER BY cost DESC;





SELECT DISTINCT mems.firstname || ' ' || mems.surname AS member,
	(SELECT recs.firstname || ' ' || recs.surname AS recommender 
		FROM cd.members recs 
     		WHERE recs.memid = mems.recommendedby)
FROM 
    cd.members mems
ORDER BY 
    member;



-- THE SAME QUERY FOR THE QUESTION PRIOR TO THE PREVIOUS QUESTION WORKS FOR THIS TOO!
SELECT 
  CONCAT(mems.firstname,' ', mems.surname) AS member, 
  facs.name AS facility, 
  CASE 
    WHEN mems.memid = 0 THEN bks.slots * facs.guestcost
    ELSE bks.slots * facs.membercost
  END AS cost
FROM
  cd.members mems
  INNER JOIN cd.bookings bks ON mems.memid = bks.memid
  INNER JOIN cd.facilities facs ON bks.facid = facs.facid
WHERE
  bks.starttime >= '2012-09-14' 
  AND bks.starttime < '2012-09-15' 
  AND (
    (mems.memid = 0 AND bks.slots * facs.guestcost > 30) OR
    (mems.memid != 0 AND bks.slots * facs.membercost > 30)
  )
ORDER BY cost DESC;