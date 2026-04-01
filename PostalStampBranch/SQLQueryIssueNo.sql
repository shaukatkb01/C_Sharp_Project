INSERT INTO StampImage (IssueNo)
SELECT IssueNo 
FROM CommStamp
WHERE IssueNo NOT IN (SELECT IssueNo FROM StampImage);