//	(c) Paul Kilfoil
//	Software Development & IT Consulting
//	www.PaulKilfoil.co.za

function CursorStyle(eltID,style)
{
	try
	{
		var h = 'auto'; // 1
		if      ( style == 2 ) h = 'help';
		else if ( style == 3 ) h = 'pointer';
		else if ( style == 4 ) h = 'progress';
		else if ( style == 5 ) h = 'not-allowed';
		else if ( style == 6 ) h = 'none';
		GetElt(eltID).style.cursor = h;
	}
	catch (x)
	{ }
}

function ToDate(dd,mm,yy)
{
	try
	{
		var h = ValidDate(dd,mm,yy,'');
		if ( h.length == 0 )
			return new Date(yy,mm-1,dd);
	}
	catch (x)
	{ }
	return null;
}

function ToInteger(theValue,defaultRet)
{
	try
	{
		var p;

		if ( defaultRet == null )
			defaultRet = 0;

		for (p = 0; p < theValue.length; p++)
			if ( ('0123456789.,').indexOf(theValue.charAt(p)) < 0 )
				return defaultRet;

		p = parseInt(theValue,10);
		if ( ! isNaN(p) )
			return p;
	}
	catch (x)
	{ }
	return defaultRet;
}

function GetElt(eltID)
{
	try
	{
		var p;
		if ( typeof(eltID) == 'object' )
			p = eltID;
		else
			p = document.getElementById(eltID);
		return p;
	}
	catch (x)
	{ }
	return null;
}

function GetEltValue(eltID)
{
	var p = GetElt(eltID);
	try
	{
		var h = Trim(p.value.toString());
		return h;
	}
	catch (x)
	{ }
	try
	{
		var k = Trim(p.innerHTML);
		return k;
	}
	catch (x)
	{ }
	return "";
}

function GetEltValueInt(eltID)
{
	try
	{
		var p = GetElt(eltID);
		var h = Trim(p.innerHTML);
		if ( h.length == 0 )
			h = p.value;
		var k = ToInteger(h);
		if ( k > 0 )
			return k;
	}
	catch (x)
	{ }
	return 0;
}

function SetEltValue(eltID,value)
{
	var p = GetElt(eltID);
	try
	{
		p.value = value;
	}
	catch (x)
	{ }
	try
	{
		p.innerHTML = value;
	}
	catch (x)
	{ }
}

function DisableElt(eltID,disable)
{
	try
	{
		var p = GetElt(eltID);
		p.disabled = disable;
		CursorStyle(p,(disable?5:1));
		if ( disable && p.title.length < 1 )
			p.title = 'Disabled';
		else if ( ! disable && p.title == 'Disabled' )
			p.title = '';
	}
	catch (x)
	{ }
}

function EltVisible(eltID)
{
	try
	{
		return GetElt(eltID).style.display.length==0;
	}
	catch (x)
	{ }
	return false;
}

function DisableForm(frmID,disable,excludeID)
{
	var k;
	var elts = GetElt(frmID).elements;
	for ( k=0 ; k < elts.length ; k++ )
	{
		if ( elts[k].id.substring(0,1) == '_' )
			continue;
		if ( excludeID == null ||
		     elts[k].id.substring(0,excludeID.length).toUpperCase() != excludeID.toUpperCase() )
			elts[k].disabled = disable;
	}
}

function Trim(theValue)
{
	return theValue.replace(/^\s+/g, '').replace(/\s+$/g, '');
}

function ValidPhone(phoneNo,country)
{
	try
	{
		phoneNo = Trim(phoneNo);

		if ( phoneNo.length < 8 )
			return false;

		if ( country == 1 ) // SA
		{
			if ( phoneNo.substring(0,1) != '0' )
				return false;
			var p = ToInteger(phoneNo,-66);
			if ( p < 100000000 || p > 999999999 ) // Valid range is 010 000 0000 to 099 999 9999
				return false;
		}
//		else
//			return ( phoneNo.substring(0,1) == '0' || phoneNo.substring(0,1) == '+' );
	}
	catch (x)
	{ }
	return true;
}

function ValidCardNumber(cardNo)
{
	try
	{
		cardNo = Trim(cardNo);
		if ( cardNo.length < 10 || cardNo.length > 20 )
			return false;
		
		var digit;
		var total = 0;
		var even  = false;

		for (var k = cardNo.length - 1; k >= 0; k--)
		{
			digit = cardNo.charAt(k);
			if ( isNaN(digit) )
				return false;
			digit = parseInt(digit, 10);
			if (even)
			{
				digit = digit * 2;
				if ( digit > 9 )
					digit = digit - 9;
			}
			total = total + digit;
			even  = ! even;
		}
		return (total % 10) == 0;
	}
	catch (x)
	{ }
	return false;
}

function ValidPIN(pin,minLength)
{
	try
	{
		pin = Trim(pin).toUpperCase();
		if ( minLength == null || minLength < 1 )
			minLength = 1;
		if ( pin.length < minLength )
			return false;
		for (var k = 0; k < pin.length; k++)
			if ( ('0123456789').indexOf(pin.charAt(k)) < 0 )
				return false;
		return true;
	}
	catch (x)
	{ }
	return false;
}

function ValidEmail(email)
{
	try
	{
		email = Trim(email);
		if ( email.length < 6 )
			return false;
		if ( email.indexOf(' ') >= 0 || email.indexOf('/') >= 0 || email.indexOf('\\') >= 0 || email.indexOf('<') >= 0 || email.indexOf('>') >= 0 || email.indexOf('(') >= 0 || email.indexOf(')') >= 0 )
			return false;
		var k = email.indexOf('@');
		if ( k < 1 )
			return false;
		var j = email.lastIndexOf('@');
		if ( k != j )
			return false;
		j = email.lastIndexOf('.');
		if ( j < k )
			return false;
		if ( email.substring(k-1,k) == '.' || email.substring(k+1,k+2) == '.' || email.substring(0,1) == '.' || email.substring(email.length-1,email.length) == '.' )
			return false;
		return true;
	}
	catch (x)
	{ }
	return false;
}

function ShowElt(eltID,show,background,retType)
{
	try
	{
		var p = GetElt(eltID);
		if (show)
		{
			p.style.visibility = "visible";
			p.style.display    = "";
			if ( background > 0 )
				document.body.className = 'greyBackground';
		}
		else
		{
			p.style.visibility = "hidden";
			p.style.display    = "none";
			if ( background > 0 )
				document.body.className = '';
		}
		if ( retType == 59 )
			return;
		return 0;
	}
	catch (x)
	{ }
	return 73;
}

function ShowBackground(show)
{
	try
	{
		if ( show > 0 )
			document.body.className = '';
		else
			document.body.className = 'greyBackground';
	}
	catch (x)
	{ }
}


function SetListValue(eltID,listValue)
{
	try
	{
		var p = GetElt(eltID);
		var k;

		for (k=0; k < p.options.length; k++)
			if ( p[k].value == listValue )
			{
				p.selectedIndex = k;
				return;
			}
		p.selectedIndex = 0;
	}
	catch (x)
	{ }
}

function GetListValueInt(eltID)
{
	try
	{
		var h = GetListValue(eltID);
		return ToInteger(h);
	}
	catch (x)
	{ }
	return 0;
}

function GetListValue(eltID)
{
	try
	{
		var p = GetElt(eltID);
		return Trim(p.options[p.selectedIndex].value);
	}
	catch (x)
	{ }
	return '';
}

function ListAdd(eltID,code,text,selected)
{
	var p   = GetElt(eltID);
	var h   = document.createElement('option');
	h.value = code.toString();
	h.text  = text.toString();
	if ( selected )
		h.selected = true;

	try
	{
		p.add(h,null); // non-IE
	}
	catch (x)
	{
		p.add(h);      // MS IE
	}
}

function ValidDate(dd,mm,yy,eltName)
{
// Standard date validation, assuming that all years ending in '00' are leap years (they aren't)

	var msg = eltName + " is not valid<br />";

	try
	{
		dd = ToInteger(dd);
		mm = ToInteger(mm);
		yy = ToInteger(yy);

		if (dd < 1 || dd > 31 || mm < 1 || mm > 12 || yy < 1900 || yy > 2999)
			return msg;
		else if (dd > 30 && (mm == 4 || mm == 6 || mm == 9 || mm == 11))
			return msg;
		else if (mm == 2 && dd > 29)
			return msg;
		else if (mm == 2 && dd == 29 && yy % 4 != 0)
			return msg;
		return "";
	}
	catch (x)
	{ }
	return msg;
}

function CheckElt(eltID,eltName,validationType,validationParm)
{
	try
	{
		var p = GetEltValue(eltID);

		if (validationType == 1 && p.length < validationParm)
			return eltName + " must contain at least " + validationParm.toString() + " characters<br />";

		if (validationType == 2 && p.length > validationParm)
			return eltName + " cannot be longer than " + validationParm.toString() + " characters<br />";

		if (validationType == 3 && p.length > 0 && !ValidEmail(p))
			return eltName + " is not valid<br />";

		return "";
	}
	catch (x)
	{ }
	return "There is a problem with " + eltName + "<br />";
}

function CheckRadio(groupName,eltName)
{
	try
	{
		var r = document.getElementsByName(groupName);
		var k = 0;
		while ( k < r.length )
			if ( r[k++].checked )
				return "";
	}
	catch (x)
	{ }
	return "You must choose one of the " + eltName + " options<br />";
}

function Validate(ctlID,lblID,eltType,eltDesc,eltMode,eltParm,eltBool)
{
	var err = "";
	var elt;
	var eltValue;

	try
	{
		if ( eltBool == null || eltBool )
		{
			eltBool = true;
			elt     = GetElt(ctlID);
			if ( elt.style.display == 'none' ) // Hidden
				eltBool = false;
		}
		if ( ! eltBool )
			return "";
		eltValue = Trim(elt.value);
	}
	catch (w)
	{
		eltValue = "";
	}

	try
	{
		if ( eltType == 1 ) // Text
		{
			if ( eltMode == 1 && eltValue.length != eltParm )
				err = eltDesc;
			else if ( eltMode == 2 && eltValue.length < eltParm )
				err = eltDesc;
			else if ( eltMode == 3 && eltValue.length > eltParm )
				err = eltDesc;
			else if ( eltMode == 4 && eltValue.length > 0 && eltValue.length != eltParm )
				err = eltDesc;
			else if ( eltMode == 5 && eltValue.length > 0 && eltValue == eltParm )
				err = eltDesc;
		}

		else if ( eltType == 2 ) // Radio
		{
			elt   = document.getElementsByName(ctlID);
			err   = eltDesc;
			var k = 0;
			while ( k < elt.length )
				if ( elt[k++].checked )
				{
					err = "";
					break;
				}
		}

		else if ( eltType == 8 ) // CheckBox
		{
			if ( eltMode == 1 && elt.checked )
				err = eltDesc;
			else if ( eltMode == 2 && ! elt.checked )
				err = eltDesc;
		}

		else if ( eltType == 3 ) // List
		{
			if ( eltMode == 73 && eltValue == eltParm )
				err = eltDesc;
			else if ( eltMode != 73 && eltValue < eltParm )
				err = eltDesc;
		}

		else if ( eltType == 4 ) // Date
			try
			{
				var h   = ( Trim(ctlID[0]) + "/" + Trim(ctlID[1]) + "/" + Trim(ctlID[2]) ).toUpperCase();
				eltDesc = eltDesc + " (" + h + ")";

				if ( eltParm == 3 && ( h == "DD/MM/YYYY" || h == "//" ) ) // Allow DD/MM/YYYY or blank
					err = "";
				else if ( ValidDate(ctlID[0],ctlID[1],ctlID[2],"").length > 0 )
					err = eltDesc;
				else if ( eltMode > 0 )
				{
					var nw = new Date(); // Today
					var dd = ToInteger(ctlID[0]);
					var mm = ToInteger(ctlID[1]);
					var yy = ToInteger(ctlID[2]);
					if ( eltMode == 1 ) // Can't be later than today
					{
						if ( nw.getFullYear() < yy )
							err = eltDesc;
						else if ( nw.getFullYear() == yy && (nw.getMonth()+1)  < mm )
							err = eltDesc;
						else if ( nw.getFullYear() == yy && (nw.getMonth()+1) == mm && nw.getDate() < dd )
							err = eltDesc;
					}
					else if ( eltMode == 2 ) // Can't be earlier than today
					{
						if ( nw.getFullYear() > yy )
							err = eltDesc;
						else if ( nw.getFullYear() == yy && (nw.getMonth()+1)  > mm )
							err = eltDesc;
						else if ( nw.getFullYear() == yy && (nw.getMonth()+1) == mm && nw.getDate() > dd )
							err = eltDesc;
					}
				}
			}
			catch (x)
			{ }

		else if ( eltType == 5 ) // EMail
		{
			if ( ! ValidEmail(eltValue) )
				err = eltDesc;
		}

		else if ( eltType == 9 ) // Card number
		{
			if ( ! ValidCardNumber(eltValue) )
				err = eltDesc;
		}

		else if ( eltType == 7 ) // Phone number
		{
			if ( eltMode == 1 && eltValue.length == 0 )
			{ }
			else if ( ! ValidPhone(eltValue,eltParm) ) // eltParm is country
				err = eltDesc;
		}

		else if ( eltType == 6 ) // Numeric
		{
			var numVal = ToInteger(eltValue,-66);
			var numLen = Trim(eltValue).length;

			if ( numVal < 0 && eltValue.length > 0 )
				err = eltDesc;
//	Value checks
			else if ( eltMode == 1 && ( numVal < 0 || numVal > eltParm ) ) // 0 <= x <= eltParm
				err = eltDesc;
			else if ( eltMode == 2 && ( numVal < 1 || numVal > eltParm ) ) // 1 <= x <= eltParm
				err = eltDesc;
			else if ( eltMode == 3 && numVal < eltParm ) // x >= eltParm
				err = eltDesc;
			else if ( eltMode == 4 && ( numVal < 0 || numVal > eltParm ) && numLen > 0 ) // 0 <= x <= eltParm, but BLANK is allowed
				err = eltDesc;
			else if ( eltMode == 5 && ( numVal < 1 || numVal > eltParm ) && numLen > 0 ) // 1 <= x <= eltParm, but BLANK is allowed
				err = eltDesc;
//	Length checks
			else if ( eltMode == 6 && numLen != eltParm ) // Must be exactly this length
				err = eltDesc;
			else if ( eltMode == 7 && ( numLen < 1 || numLen > eltParm ) ) // 1 <= x <= eltParm
				err = eltDesc;
			else if ( eltMode == 8 && numLen < eltParm ) // x <= eltParm
				err = eltDesc;
//	Credit card checks
			else if ( eltMode >= 71 && eltMode <= 74 )
			{
				if ( eltValue.length != numLen )
					err = eltDesc;
				else if ( eltMode == 71 && ( numLen < 13 || numLen > 19 ) ) // Visa
					err = eltDesc;
				else if ( eltMode == 72 && numLen != 16 ) // MC
					err = eltDesc;
				else if ( eltMode == 73 && numLen != 15 ) // AmEx
					err = eltDesc;
				else if ( eltMode == 74 && ( numLen < 14 || numLen > 16 ) ) // Diners
					err = eltDesc;
			}
			else if ( eltMode == 66 ) // SA Id
			{
				if ( eltParm == 3 && numLen == 0 ) // Allow blank
				{ }
				else if ( numLen != 13 )
					err = eltDesc;
				else if ( ValidDate(eltValue.substring(4,6),eltValue.substring(2,4),'19'+eltValue.substring(0,2),'').length > 0 )
					err = eltDesc;
			}
			else if ( eltMode > 99 && ( numVal < eltMode || numVal > eltParm ) ) // So a number between (eg) 1900 and 2017
				err = eltDesc;
		}

		else if ( eltType == 81 ) // Show error
			err = eltDesc;

		else if ( eltType == 91 ) // Hide error
			err = "";

		SetErrorLabel(lblID,37,err);

		try
		{
			if ( err.length > 0 )
				elt.style.borderColor = colorErr;
			else
				elt.style.borderColor = colorOK;
		}
		catch (w)
		{ }
	}
	catch (z)
	{
		alert(z);
	}
	return err + ( err.length == 0 ? '' : '<br />' );
}

function SetErrorLabel(lblID,onOrOff,value,hint)
{
	try
	{
		var lbl = GetElt(lblID);
		if ( onOrOff == 0 )
			lbl.className = '';
		else
			lbl.className = 'Error';
		if ( value != null )
			SetEltValue(lbl,value);
		if ( hint != null )
			lbl.title = hint;
	}
	catch (x)
	{ }
}

function ShowPopup(eltID,info,event,endLR,eltObj)
{
	try
	{
		if ( info.length > 0 )
		{
			var q = GetElt(eltID);
			q.style.visibility = "visible";
			q.style.display = '';
			q.innerHTML = info;
			if ( event != null )
			{
				q.style.left  = "auto";
				q.style.right = "auto";
				q.style.top   = event.clientY.toString() + "px";
				if ( endLR == 'R' )
					q.style.right = (window.innerWidth-event.clientX-10).toString() + "px";
				else
					q.style.left  = (event.clientX+10).toString() + "px";
			}
			if ( eltObj != null )
			{
				var ctl = eltObj.getBoundingClientRect();
				var scr = document.body.getBoundingClientRect();
				q.style.top  = ( ctl.top  - scr.top  + 15 ).toString()  + "px";
				q.style.left = ( ctl.left - scr.left + 15 + (ctl.right-ctl.left)/2 ).toString() + "px";
			}
		}
		else
			ShowElt(eltID,false);
	}
	catch (x)
	{ }
}
