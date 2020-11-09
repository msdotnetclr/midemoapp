CREATE USER midemoapp FROM EXTERNAL PROVIDER 

ALTER ROLE db_datareader ADD MEMBER midemoapp -- gives permission for normal app read to database
ALTER ROLE db_datawriter ADD MEMBER midemoapp -- gives permission for normal app write to database
