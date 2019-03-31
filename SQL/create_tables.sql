drop table dbo.db_companies;

create table	dbo.db_companies
(
	symbol			nvarchar(8)		NOT NULL
	, [name]		nvarchar(1024)	NULL
	, [date]		nvarchar(1024)	NULL
	, isEnabled		bit				NULL
	, [type]		nvarchar(1024)	NULL
	, [iexID]		nvarchar(1024)	NULL
	, [userOption]	nvarchar(1024)	NULL
	, [name_chart]	nvarchar(1024)	NULL
	, [pricingData]	nvarchar(4000)	NULL
	CONSTRAINT db_companies_symbol	PRIMARY KEY CLUSTERED
	(
		symbol ASC
	)
)

-- drop table dbo.db_prices;

create table dbo.db_prices
(
	symbol					nvarchar(8)		NOT NULL
	, [date]				nvarchar(64)	NOT NULL
	, [open]				float(12)		NULL
	, [high]				float(12)		NULL
	, [low]					float(12)		NULL
	, [close]				float(12)		NULL
	, [volume]				INT				NULL
	, [unadjustedVolume]	INT				NULL
	, [change]				float(12)		NULL
	, [changePercent]		float(12)		NULL
	, [vwap]				float(12)		NULL
	, [label]				nvarchar(1024)	NULL
	, [changeOverTime]		float(12)		NULL
	, EquityID				INT				NULL
	
	CONSTRAINT db_prices_symbol	PRIMARY KEY CLUSTERED
	(
		symbol ASC, [date]
	)
)

-- drop table dbo.db_fx;
create table dbo.db_fx
(
	[date]					nvarchar(64)	not null
	,[USD]					float(12)		NULL
	,[GBP]					float(12)		NULL
	,[EUR]					float(12)		NULL
	,[JPY]					float(12)		NULL

	CONSTRAINT db_fx_date PRIMARY KEY CLUSTERED
	(
		[date] ASC
	)
)


create table dbo.db_news
(
	symbol					nvarchar(8)		NOT NULL
	,[date]					nvarchar(64)	not null
	,[headline]				nvarchar(4000)	 null
	,[url]					nvarchar(4000)	 null

	CONSTRAINT db_news_symbol	PRIMARY KEY CLUSTERED
	(
		symbol ASC, [date]
	)
)


create table dbo.db_logo
(
	symbol					nvarchar(8)		NOT NULL
	, logo_image			varbinary(max)	not null 	
	
	CONSTRAINT db_logo_symbol	PRIMARY KEY CLUSTERED
	(
		symbol ASC
	)			
)