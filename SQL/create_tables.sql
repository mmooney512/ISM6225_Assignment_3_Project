
create table	dbo.db_companies
(
	symbol			nvarchar(8)		NOT NULL
	, [name]		nvarchar(1024)	NULL
	, [date]		nvarchar(1024)	NULL
	, isEnabled		nvarchar(1024)	NULL
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

create table dbo.db_prices
(
	symbol					nvarchar(8)		NOT NULL
	, EquityID				INT				NULL
	, [date]				nvarchar(1024)	NULL
	, [open]				numeric(12,4)	NULL
	, [high]				numeric(12,4)	NULL
	, [low]					numeric(12,4)	NULL
	, [close]				numeric(12,4)	NULL
	, [volume]				numeric(12,4)	NULL
	, [unadjustedVolume]	numeric(12,4)	NULL
	, [change]				numeric(12,4)	NULL
	, [changePercent]		numeric(12,4)	NULL
	, [vwap]				numeric(12,4)	NULL
	, [label]				nvarchar(1024)	NULL
	, [changeOverTime]		numeric(12,4)	NULL

	CONSTRAINT db_prices_symbol	PRIMARY KEY CLUSTERED
	(
		symbol ASC
	)
)

