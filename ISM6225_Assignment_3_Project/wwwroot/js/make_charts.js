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

function PageReloaded()
{
    try
    {
        new Chart(document.getElementById("chart_one"),
            {
                type: 'bar',
                data: {
                    labels: ["2018", "2019"],
                    datasets: [
                        {
                            label: "Amount (USD)",
                            backgroundColor: ["#3e95cd", "#3cba9f"],
                            data: [1153, 1180]
                        }
                    ]
                },
                options: {
                    legend: { display: false },
                    title: {
                        display: true,
                        text: 'Americans who plan to go on vacation will spend on average, per person:'
                    },
                    scales: {
                        yAxes: [{
                            display: true,
                            ticks: {
                                suggestedMin: 0,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });

        new Chart(document.getElementById("chart_two"),
            {
                type: 'horizontalBar',
                data: {
                    labels: ["Cash, Check, or Savings", "Credit Card (to pay in full)", "Reward Points", "Credit Card (to pay monthly)", "A Tax Refund", "Timeshare/Vacation Plan", "Other", "Not Sure"],
                    datasets: [
                        {
                            label: "Percentage (%)",
                            backgroundColor: ["#FF9933", "#FF9933", "#FF9933", "#FF9933", "#FF9933", "#FF9933", "#FF9933", "#FF9933" ],
                            data: [71,33,19,18,13,7,2,5]
                        }
                    ]
                },
                options: {
                    legend: { display: false },
                    title: {
                        display: true,
                        text: 'How are people paying for their vacation'
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            ticks: {
                                suggestedMin: 0,
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });

        new Chart(document.getElementById("chart_three"),
            {
                type: 'doughnut',
                data: {
                    labels: ["No Summer Plans", "Summer Plans"],
                    datasets: [
                        {
                            label: "Percent (Americans)",
                            backgroundColor: ["#FFFF99", "#8e5ea2"],
                            data: [41, 59]
                        }
                    ]
                },
                options: {
                    legend: { display: true },
                    title: {
                        display: true,
                        text: 'Americans who plan to go on summer vacation:'
                    }
                 
                }
            });
    }
    catch (e)
    {
        ExceptionHandler(e);
    }
}