var DebugMode = true;       //set to true or false to see or hide the alert boxes.
//----------------------------------------------------------------------------------------------------------------------------
//ErrorHandler: Global Error Handler for all the javascript code
//called by:    all .js fx
//inputs:       error object
//outputs:      alert box if in debug mode -- else silenty fails
//updates:      2019-03-10 :: mm :: original file
//----------------------------------------------------------------------------------------------------------------------------
function ExceptionHandler(e, str_AdditionalMessage) {
    if (str_AdditionalMessage === undefined) { str_AdditionalMessage = "."; }
    if (DebugMode) {
        var ParseError = "fx " + e.constructor + "  generated an error \n" + e.name + " :: " + e.message + "\n Line: " + e.lineNumber + " FileName: " + e.fileName;
        ParseError = ParseError + "\n\n" + str_AdditionalMessage;
        alert(ParseError);
        throw (null);
    }
    else {
        throw ("An unanticipated error occured");
    }

}

function DrawChart_l()
{
    try
    {
        new Chart(document.getElementById("chart_stock"),
            {
                type: 'line',
                data:
                {
                    labels: ["2018", "2019"],
                    datasets: [{
                        data: [800, 1200],
                        label: "one",
                        borderColor: "#3cba9f",
                        fill: false
                        }]
                },
                options: {
                    title:
                    {
                        display: true,
                        text: "sample line"
                    }
                }

            }
        );


    }
    catch (e) {
        ExceptionHandler(e);
    }
}

function DrawCharts(stock_symbol, stock_name, labels_dates, data_prices)
{
    try
    {
        new Chart(document.getElementById("chart_stock"),
            {
                type: 'line',
                data:
                {
                    labels: labels_dates.split(",") ,
                    datasets: [{
                        data: data_prices.split(","),
                        label: stock_symbol,
                        borderColor: "#3cba9f",
                        fill: false
                    }]
                },
                options: {
                    title:
                    {
                        display: true,
                        text: stock_name
                    }
                }

            }
        );
    }
    catch (e)
    {
        ExceptionHandler(e);
    }
}

function DrawXeChart(xe_exchange_rate, labels_dates, data_rates)
{
    try
    {
        new Chart(document.getElementById("chart_xe"),
            {
                type: 'line',
                data:
                {
                    labels: labels_dates.split(","),
                    datasets: [{
                        data: data_rates.split(","),
                        label: xe_exchange_rate,
                        borderColor: "#3cba9f",
                        fill: false
                    }]
                },
                options: {
                    title:
                    {
                        display: true,
                        text: "Exchange Rate: " + xe_exchange_rate
                    }
                }

            }
        );
    }
    catch (e) {
        ExceptionHandler(e);
    }
}