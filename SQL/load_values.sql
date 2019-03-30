
TRUNCATE TABLE		dbo.db_companies;


INSERT INTO dbo.db_companies
(Symbol , [name] )
VALUES
('AAL'		,'American Airlines Group Inc.')
,('CAR'		,'Avis Budget Group Inc.')	
,('DAL'		,'Delta Air Lines Inc.')	
,('H'		,'Hyatt Hotels Corporation Class A')	
,('HA'		,'Hawaiian Holdings Inc.')	
,('HLT'		,'Hilton Worldwide Holdings Inc.')	
,('HST'		,'Host Hotels &amp; Resorts Inc.')	
,('HTZ'		,'Hertz Global Holdings Inc')	
,('LUV'		,'Southwest Airlines Company')	
,('MAR'		,'Marriott International')	
,('UAL'		,'United Continental Holdings Inc.')	
,('WH'		,'Wyndham Hotels &amp; Resorts Inc.')	

UPDATE [dbo].[db_companies]
SET isEnabled = 1;

