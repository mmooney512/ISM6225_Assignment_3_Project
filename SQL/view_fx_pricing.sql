CREATE VIEW		view_fx_pricing AS
(
SELECT			db_prices.symbol
				, db_prices.[date]
				, db_prices.[close]

				, db_prices.[close] * db_fx.USD	AS close_USD
				, db_prices.[close] * db_fx.GBP	AS close_GBP
				, db_prices.[close] * db_fx.EUR	AS close_EUR
				, db_prices.[close] * db_fx.JPY	AS close_JPY

FROM		dbo.db_prices
			INNER JOIN dbo.db_fx
				ON db_prices.[date] = db_fx.[date]
);

			

CREATE VIEW		view_fx_rates AS
(
SELECT			db_fx.[date]	
				, db_fx.USD / db_fx.GBP		AS GBP_USD
				, db_fx.GBP / db_fx.USD 	AS USD_GBP

				, db_fx.USD / db_fx.EUR		AS EUR_USD
				, db_fx.EUR / db_fx.USD 	AS USD_EUR

				, db_fx.USD / db_fx.JPY		AS JPY_USD
				, db_fx.JPY / db_fx.USD 	AS USD_JPY

FROM			dbo.db_fx

);
