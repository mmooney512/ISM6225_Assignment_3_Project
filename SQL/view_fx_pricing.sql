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

			
