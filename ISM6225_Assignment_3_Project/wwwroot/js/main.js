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

//----------------------------------------------------------------------------
// hasClass

//updates:           2019-03-10 :: mm :: original file
//----------------------------------------------------------------------------

function hasClass(elem, className)
{
    return new RegExp(' ' + className + ' ').test(' ' + elem.className + ' ');
}


//----------------------------------------------------------------------------
// toggleClass

//----------------------------------------------------------------------------
function toggleClass(elem, className)
{
    var newClass = ' ' + elem.className.replace(/[\t\r\n]/g, ' ') + ' ';
    if (hasClass(elem, className)) {
        while (newClass.indexOf(' ' + className + ' ') >= 0) {
            newClass = newClass.replace(' ' + className + ' ', ' ');
        }
        elem.className = newClass.replace(/^\s+|\s+$/g, '');
    } else {
        elem.className += ' ' + className;
    }
}




//------------------------------------------------------------------------------------------------------------------------------
//page_reloaded:    after the page has been refreshed
//called by:        index.html/body onload="page_reloaded()"
//uses:
//calls:            
//
//updates:           2019-03-10 :: mm :: original file
//
//------------------------------------------------------------------------------------------------------------------------------
function PageReloaded() {
    try
    {
        // Create a new <div> give the div the class name nav-mobile
        // and append the dive to the ".nav" element
        var mobile = document.createElement('div');
        mobile.className = 'nav-mobile';
        document.querySelector('.nav').appendChild(mobile);

        // Mobile nav function
        // clicking the icon flips the visibility of the menu
        var mobileNav = document.querySelector('.nav-mobile');
        var toggle = document.querySelector('.nav-list');
        mobileNav.onclick = function ()
        {
            toggleClass(this, 'nav-mobile-open');
            toggleClass(toggle, 'nav-active');
        }

    }
    catch (e) {
        ExceptionHandler(e);
    }
}



// Toggle between adding and removing the "responsive" 
// class to topnav when the user clicks on the icon 

function myFunction()
{
    var x = document.getElementById("myTopnav");
    if (x.className === "topnav")
    {
        x.className += " responsive";
    } else
    {
        x.className = "topnav";
    }
}

function myFunction_about() {
    var x = document.getElementById("myTopnav");
    if (x.className === "topnav") {
        x.className += " responsive";
    } else {
        x.className = "topnav";
    }
    var y = document.getElementById("hdr_menu");
    if (y.className === "hdr one columns") {
        y.className ="hdr nine columns"
    }
    else {
        y.className = "hdr one columns"
    }
    
}