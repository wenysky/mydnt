﻿/*
	[Discuz!] (C)2001-2007 Comsenz Inc.
	This is NOT a freeware, use is subject to license terms

	$RCSfile: post.js,v $
	$Revision: 1.24 $
	$Date: 2007/07/20 12:20:51 $
*/

var postSubmited = false;
var smdiv = new Array();

function AddText(txt) {
    try {
        obj = typeof $('postform').message != 'undefined' ? $('postform').message : $('e_textarea');
    } catch (e) { 
		obj = typeof $('quickpostform').message != 'undefined' ? $('quickpostform').message : $('quickpostmessage');
	}
	selection = document.selection;
	checkFocus();
	if(!isUndefined(obj.selectionStart)) {
		var opn = obj.selectionStart + 0;
		obj.value = obj.value.substr(0, obj.selectionStart) + txt + obj.value.substr(obj.selectionEnd);
	} else if(selection && selection.createRange) {
		var sel = selection.createRange();
		sel.text = txt;
		sel.moveStart('character', -strlen(txt));
	} else {
		obj.value += txt;
	}
}

function checkFocus() {
    var textarea;
    try {
        textarea = typeof $('postform').message != 'undefined' ? $('postform').message : $('e_textarea');
    } catch (e) { 
		textarea = typeof $('quickpostform').message != 'undefined' ? $('quickpostform').message : $('quickpostmessage');
	}
    var obj = typeof wysiwyg == 'undefined' || !wysiwyg ? textarea : editwin;
	if(!obj.hasfocus) {
		obj.focus();
	}
}

function ctlent(event) {
	if(postSubmited == false && (event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83) && $('postsubmit')) {
		if(in_array($('postsubmit').name, ['topicsubmit', 'replysubmit', 'editsubmit']) && !validate($('postform'))) {
			doane(event);
			return;
		}
		postSubmited = true;
		$('postsubmit').disabled = true;
		$('postform').submit();
	}
}

function ctltab(event) {
	if(event.keyCode == 9) {
		doane(event);
	}
}

function ctlentParent(event) {
    var pForm = parent.window.document.getElementById('postform');
    var pSubmit = parent.window.document.getElementById('postsubmit');

	if(postSubmited == false && (event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83) && pSubmit) {
		if (parent.window.validate && !parent.window.validate(pForm))
		{
			doane(event);
			return;
		}
		postSubmited = true;
		pSubmit.disabled = true;
		pForm.submit();
	}
}

function deleteData() {
	if(is_ie) {
		saveData('', 'delete');
	} else if(window.sessionStorage) {
		try {
			sessionStorage.removeItem('Discuz!');
		} catch(e) {}
	}
}

function insertSmiley(smilieid) {
	checkFocus();
	var src = $('smilie_' + smilieid).src;
	var code = $('smilie_' + smilieid).alt;
	if(typeof wysiwyg != 'undefined' && wysiwyg && allowsmilies && (!$('smileyoff') || $('smileyoff').checked == false)) {
		if(is_moz) {
			applyFormat('InsertImage', false, src);
			
			var smilies = editdoc.body.getElementsByTagName('img');
			for(var i = 0; i < smilies.length; i++) {
				if(smilies[i].src == src && smilies[i].getAttribute('smilieid') < 1) {
					smilies[i].setAttribute('smilieid', smilieid);
					smilies[i].setAttribute('border', "0");
				}
			}
		} else {
			insertText('<img src="' + src + '" border="0" smilieid="' + smilieid + '" alt="" /> ', false);
		}
	} else {
		code += ' ';
		AddText(code);
	}
	hideMenu();
}

function smileyMenu(ctrl) {
	ctrl.style.cursor = 'pointer';
	if(ctrl.alt) {
		ctrl.pop = ctrl.alt;
		ctrl.alt = '';
	}
	if(ctrl.title) {
		ctrl.lw = ctrl.title;
		ctrl.title = '';
	}
	//if(!smdiv[ctrl.id]) {
		smdiv[ctrl.id] = document.createElement('div');
		smdiv[ctrl.id].id = ctrl.id + '_menu';
		smdiv[ctrl.id].style.display = 'none';
		smdiv[ctrl.id].style.width = '60px';
		smdiv[ctrl.id].style.height = '60px';
		smdiv[ctrl.id].className = 'popupmenu_popup';
		ctrl.parentNode.appendChild(smdiv[ctrl.id]);
	//}
	smdiv[ctrl.id].innerHTML = '<table width="100%" height="100%"><tr><td align="center" valign="middle"><img src="' + ctrl.src + '" border="0" /></td></tr></table>';
	showMenu(ctrl.id, 0, 0, 1, 0);
}



function showsmiles(index, typename, pageindex, seditorKey)
{
	$("s_" + index).className = "current";
	var cIndex = 1;
	for (i in smilies_HASH) {
		if (cIndex != index) {
			$("s_" + cIndex).className = "";
		}
		$("s_" + cIndex).style.display = "";
		cIndex ++;
	}

	var pagesize = (typeof smiliesCount) == 'undefined' ? 12 : smiliesCount;
	var url = (typeof forumurl) == 'undefined' ? '' : forumurl;
	var s = smilies_HASH[typename];
	var pagecount = Math.ceil(s.length/pagesize);
	var inseditor = typeof seditorKey != 'undefined';

	if (isUndefined(pageindex)) {
		pageindex = 1;
	}
	if (pageindex > pagecount) {
		pageindex = pagecount;
	}

	var maxIndex = pageindex*pagesize;
	if (maxIndex > s.length) {
		maxIndex = s.length;
	}
	maxIndex = maxIndex - 1;

	var minIndex = (pageindex-1)*pagesize;

	var html = '<table id="' + index + '_table" cellpadding="0" cellspacing="0" style="clear: both"><tr>';

	var ci = 1;
	for (var id = minIndex; id <= maxIndex; id++) {
		var clickevt = 'insertSmiley(\'' + addslashes(s[id]['code']) + '\');';
		if (inseditor) {
			clickevt = 'seditor_insertunit(\'' + seditorKey + '\', \'' + s[id]['code'] + '\');';
		}

		html += '<td valign="middle"><img style="cursor: pointer;" src="' + url + 'editor/images/smilies/' + s[id]['url'] + '" id="smilie_' + s[id]['code'] + '" alt="' + s[id]['code'] + '" onclick="' + clickevt + '" onmouseover="smilies_preview(\'s\', this, 40)" onmouseout="smilies_preview(\'s\')" title="" border="0" height="20" width="20" /></td>';
		if (ci%colCount == 0) {
			html += '</tr><tr>'
		}
		ci ++;
	}

	html += '<td colspan="' + (colCount - ((ci-1) % colCount)) + '"></td>';
	html += '</tr>';
	html += '</table>';
	$(seditorKey+"showsmilie").innerHTML = html;

	if (pagecount > 1) {
		html = '<div class="p_bar">';
		for (var i = 1; i <= pagecount; i++) {
			if (i == pageindex) {
				html += "<a class=\"p_curpage\">" + i + "</a>";
			}
			else {
				html += "<a class=\"p_num\" href='#smiliyanchor' onclick=\"showsmiles(" + index + ", '" + typename + "', " + i + ", '"+seditorKey+"')\">" + i + "</a>"
			}
		}
		html += '</div>'
		$(seditorKey+"showsmilie_pagenum").innerHTML = html;
	}
	else {
		$(seditorKey+"showsmilie_pagenum").innerHTML = "";
	}
}

function showFirstPageSmilies(firstpagesmilies, defaulttypename, maxcount, seditorKey)
{
	var html = '<table align="center" border="0" cellpadding="3" cellspacing="0" width="90%"><tr>';
	var ci = 1;
	var inseditor = (typeof seditorKey != 'undefined');
	var url = (typeof forumurl) == 'undefined' ? '' : forumurl;
	var s = firstpagesmilies[defaulttypename];
	for (var id = 0; id <= maxcount - 1; id++) {
		if(s[id] == null)
			continue;
		var clickevt = 'insertSmiley(\'' + addslashes(s[id]['code']) + '\');';
		if (inseditor) {
			clickevt = 'seditor_insertunit(\'' + seditorKey + '\', \'' + s[id]['code'] + '\');';
		}
		html += '<td valign="middle"><img style="cursor: pointer;" src="' + url + 'editor/images/smilies/' + s[id]['url'] + '" id=smilie_' + s[id]['code'] + ' alt="' + s[id]['code'] + '" onclick="' + clickevt + '" onmouseover="smilies_preview(\'s\', this, 40)" onmouseout="smilies_preview(\'s\')" title="" border="0" height="20" width="20" /></td>';
		if (ci%4 == 0) {
			html += '</tr><tr>'
		}
		ci ++;
	}
	html += '<td colspan="' + (4 - ((ci-1) % 4)) + '"></td>';
	html += '</tr>';
	html += '</table>';

	$("showsmilie").innerHTML = html;
}

function scrollSmilieTypeBar(bar, scrollwidth)
{
	//bar.scrollLeft += scrollwidth;
	var i = 0;
	if (scrollwidth > 0) {
		var scl = window.setInterval(function(){
			if (i < scrollwidth) {
				bar.scrollLeft += 1;
				i++
			}
			else
				window.clearInterval(scl);
		}, 1);
	}
	else {
		var scl = window.setInterval(function(){
			if (i > scrollwidth) {
				bar.scrollLeft -= 1;
				i--
			}
			else
				window.clearInterval(scl);
		}, 1);
	}
}
function smilies_preview(id, obj, v) {
	if(!obj) {
		$(id + '_preview_table').style.display = 'none';
	} else {
		$(id + '_preview_table').style.display = '';
		$(id + '_preview').innerHTML = '<img src="' + obj.src + '" />';
	}
}

/*Discuz!NT end*/
function parseurl(str, mode) {
	str = str.replace(/([^>=\]"'\/]|^)((((https?|ftp):\/\/)|www\.)([\w\-]+\.)*[\w\-\u4e00-\u9fa5]+\.([\.a-zA-Z0-9]+|\u4E2D\u56FD|\u7F51\u7EDC|\u516C\u53F8)((\?|\/|:)+[\w\.\/=\?%\-&~`@':+!]*)+\.(jpg|gif|png|bmp))/ig, mode == 'html' ? '$1<img src="$2" border="0">' : '$1[img]$2[/img]');
	str = str.replace(/([^>=\]"'\/@]|^)((((https?|ftp|gopher|news|telnet|rtsp|mms|callto|bctp|ed2k|thunder|synacast):\/\/))([\w\-]+\.)*[:\.@\-\w\u4e00-\u9fa5]+\.([\.a-zA-Z0-9]+|\u4E2D\u56FD|\u7F51\u7EDC|\u516C\u53F8)((\?|\/|:)+[\w\.\/=\?%\-&~`@':+!#]*)*)/ig, mode == 'html' ? '$1<a href="$2" target="_blank">$2</a>' : '$1[url]$2[/url]');
	str = str.replace(/([^\w>=\]"'\/@]|^)((www\.)([\w\-]+\.)*[:\.@\-\w\u4e00-\u9fa5]+\.([\.a-zA-Z0-9]+|\u4E2D\u56FD|\u7F51\u7EDC|\u516C\u53F8)((\?|\/|:)+[\w\.\/=\?%\-&~`@':+!#]*)*)/ig, mode == 'html' ? '$1<a href="$2" target="_blank">$2</a>' : '$1[url]$2[/url]');
	str = str.replace(/([^\w->=\]:"'\.\/]|^)(([\-\.\w]+@[\.\-\w]+(\.\w+)+))/ig, mode == 'html' ? '$1<a href="mailto:$2">$2</a>' : '$1[email]$2[/email]');
	return str;
}

function getData(tagname) {
	if (typeof tagname == 'undefined' || tagname == '')
	{
		tagname = 'Discuz!';
	}
	var message = '';
	if(is_ie) {
		try {
			textobj.load(tagname);
			var oXMLDoc = textobj.XMLDocument;
			var nodes = oXMLDoc.documentElement.childNodes;
			message = nodes.item(nodes.length - 1).getAttribute('message');
		} catch(e) {}
	} else if(window.sessionStorage) {
		try {
		    message = sessionStorage.getItem(tagname);
		    if (!message)
		        message = "";
		} catch(e) {}
	}
	return message.toString();

}

/* function loadData(silent) {
alert("post.js");
	var message = '';
	message = getData('Discuz!');
	if (!silent)
	{
		if(in_array((message = trim(message)), ['', 'null', 'false', null, false])) {
			alert(lang['post_autosave_none']);
			return;
		}
		if(!confirm(lang['post_autosave_confirm'])) {
			return;
		}
	}

	var formdata = message.split(/\x09\x09/);
	for(var i = 0; i < $('postform').elements.length; i++) {
		var el = $('postform').elements[i];
		if(el.name != '' && (el.tagName == 'TEXTAREA' || el.tagName == 'INPUT' && el.type == 'text')) {
			for(var j = 0; j < formdata.length; j++) {
				var ele = formdata[j].split(/\x09/);
				if(ele[0] == el.name) {
					elvalue = !isUndefined(ele[3]) ? ele[3] : '';
					if(ele[1] == 'INPUT') {
						if(ele[2] == 'text') {
							el.value = elvalue;
						} else if(ele[2] == 'checkbox' || ele[2] == 'radio') {
							el.checked = true;
						}
					} else if(ele[1] == 'TEXTAREA') {
						if(ele[0] == 'message') {
							if(typeof wysiwyg == 'undefined' || !wysiwyg) {
								textobj.value = elvalue;
							} else {
								editdoc.body.innerHTML = bbcode2html(elvalue);
							}
						} else {
							el.value = elvalue;
						}
					}
					break
				}
			}
		}
	}
}
 */
function setData(data, tagname) {
	if (typeof tagname == 'undefined' || tagname == '')
	{
		tagname = 'Discuz!';
	}
	if(is_ie) {
		try {
			var oXMLDoc = textobj.XMLDocument;
			var root = oXMLDoc.firstChild;
			if(root.childNodes.length > 0) {
				root.removeChild(root.firstChild);
			}
			var node = oXMLDoc.createNode(1, 'POST', '');
			var oTimeNow = new Date();
			oTimeNow.setHours(oTimeNow.getHours() + 24);
			textobj.expires = oTimeNow.toUTCString();
			node.setAttribute('message', data);
			oXMLDoc.documentElement.appendChild(node);
			textobj.save(tagname);
		} catch(e) {}
	} else if(window.sessionStorage) {
		try {
			sessionStorage.setItem(tagname, data);
		} catch(e) {}
	}

}

/* function saveData(data, del) {
	if(!data && typeof del == 'undefined') {
		return;
	}

	if(typeof wysiwyg != 'undefined' && typeof editorid != 'undefined' && typeof bbinsert != 'undefined' && bbinsert && $(editorid + '_mode') && $(editorid + '_mode').value == 1) {
		data = html2bbcode(data);
	}

	var formdata = '';
	for(var i = 0; i < $('postform').elements.length; i++) {
		var el = $('postform').elements[i];
		if(el.name != '' && (el.tagName == 'TEXTAREA' || el.tagName == 'INPUT' && el.type == 'text') && el.name.substr(0, 6) != 'attach') {
			var elvalue = el.name == 'message' ? data : el.value;
			formdata += el.name + String.fromCharCode(9) + el.tagName + String.fromCharCode(9) + el.type + String.fromCharCode(9) + elvalue + String.fromCharCode(9, 9);
		}
	}

	setData(formdata, 'Discuz!');
} */
/* function saveData(ignoreempty) {

	var ignoreempty = isUndefined(ignoreempty) ? 0 : ignoreempty;

	var obj = $('postform') && (($('fwin_newthread') && $('fwin_newthread').style.display == '') || ($('fwin_reply') && $('fwin_reply').style.display == '')) ? $('postform') : ($('fastpostform') ? $('fastpostform') : $('postform'));

	if(!obj) return;

	var data = subject = message = '';

	for(var i = 0; i < obj.elements.length; i++) {

		var el = obj.elements[i];

		if(el.name != '' && (el.tagName == 'TEXTAREA' || el.tagName == 'INPUT' && (el.type == 'text' || el.type == 'checkbox' || el.type == 'radio')) && el.name.substr(0, 6) != 'attach') {

			var elvalue = el.value;

			if(el.name == 'subject') {

				subject = trim(elvalue);

			} else if(el.name == 'message') {

				if(typeof wysiwyg != 'undefined' && wysiwyg == 1) {

					elvalue = html2bbcode(editdoc.body.innerHTML);

				}

				message = trim(elvalue);

			}

			if((el.type == 'checkbox' || el.type == 'radio') && !el.checked) {

				continue;

			}

			if(trim(elvalue)) {

				data += el.name + String.fromCharCode(9) + el.tagName + String.fromCharCode(9) + el.type + String.fromCharCode(9) + elvalue + String.fromCharCode(9, 9);

			}

		}

	}



	if(!subject && !message && !ignoreempty) {

		return;

	}



	saveUserdata('forum', data);

} */
/* function saveUserdata(name, data) {

	if(BROWSER.ie){

		with(document.documentElement) {

			setAttribute("value", data);

			save('Discuz_' + name);

		}

	} else if(window.sessionStorage){

		sessionStorage.setItem('Discuz_' + name, data);

		}

} */


var autosaveDatai, autosaveDatatime;
function autosaveData(op) {
	var autosaveInterval = 60;
	obj = $(editorid + '_cmd_autosave');
	if(op) {
		if(op == 2) {
			saveData(wysiwyg ? editdoc.body.innerHTML : textobj.value);
		} else {
			setcookie('disableautosave', '', -2592000);
		}
		autosaveDatatime = autosaveInterval;
		autosaveDatai = setInterval(function() {
			autosaveDatatime--;
			if(autosaveDatatime == 0) {
				saveData(wysiwyg ? editdoc.body.innerHTML : textobj.value);
				autosaveDatatime = autosaveInterval;
			}
			if($('autsavet')) {
				$('autsavet').innerHTML = '(' + autosaveDatatime + '秒' + ')';
			}
		}, 1000);
		obj.onclick = function() { autosaveData(0); }
	} else {
		setcookie('disableautosave', 1, 2592000);
		clearInterval(autosaveDatai);
		$('autsavet').innerHTML = '(已停止)';
		obj.onclick = function() { autosaveData(1); }
	}
}

function setCaretAtEnd() {
	if(typeof wysiwyg != 'undefined' && wysiwyg) {
		editdoc.body.innerHTML += '';
	} else {
		editdoc.value += '';
	}
}

function storeCaret(textEl){
	if(textEl.createTextRange){
		textEl.caretPos = document.selection.createRange().duplicate();
	}
}

if (BROWSER.ie >= 5 || is_moz >= 2) {
	window.onbeforeunload = function () {
		try {
			saveData(wysiwyg && bbinsert ? editdoc.body.innerHTML : textobj.value);
		} catch(e) {}
	};
}

function insertmedia() {
	InFloat = InFloat_Editor;
	if(is_ie) $(editorid + '_mediaurl').pos = getCaret();
	showMenu(editorid + '_popup_media', true, 0, 2);
}

function setmediacode(editorid) {
	checkFocus();
	if(is_ie) setCaret($(editorid + '_mediaurl').pos);
	insertText('[media='+$(editorid + '_mediatype').value+
		','+$(editorid + '_mediawidth').value+
		','+$(editorid + '_mediaheight').value+']'+
		$(editorid + '_mediaurl').value+'[/media]');
	$(editorid + '_mediaurl').value = '';
	hideMenu();
}


function setmediatype(editorid) {
	var ext = $(editorid + '_mediaurl').value.lastIndexOf('.') == -1 ? '' : $(editorid + '_mediaurl').value.substr($(editorid + '_mediaurl').value.lastIndexOf('.') + 1, $(editorid + '_mediaurl').value.length).toLowerCase();
	if(ext == 'rmvb') {
		ext = 'rm';
	}
	if($(editorid + '_mediatyperadio_' + ext)) {
		$(editorid + '_mediatyperadio_' + ext).checked = true;
		$(editorid + '_mediatype').value = ext;
	}
}

var divdragstart = new Array();
function divdrag(e, op, obj) {
	if(op == 1) {
		divdragstart = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
		divdragstart[2] = parseInt(obj.style.left);
		divdragstart[3] = parseInt(obj.style.top);
		doane(e);
	} else if(op == 2 && divdragstart[0]) {
		var divdragnow = is_ie ? [event.clientX, event.clientY] : [e.clientX, e.clientY];
		obj.style.left = (divdragstart[2] + divdragnow[0] - divdragstart[0]) + 'px';
		obj.style.top = (divdragstart[3] + divdragnow[1] - divdragstart[1]) + 'px';
		doane(e);
	} else if(op == 3) {
		divdragstart = [];
		doane(e);
	}
}

function pagescrolls(op) {
	if(!infloat && op.substr(0, 6) == 'credit') {
		window.open('faq.php?action=credits&fid=' + fid);
		return;
	}
	switch(op) {
		case 'credit1':hideMenu();$('moreconf').style.display = 'none';$('extcreditbox1').innerHTML = $('extcreditbox').innerHTML;pagescroll.left();break;
		case 'credit2':$('moreconf').style.display = 'none';$('extcreditbox2').innerHTML = $('extcreditbox').innerHTML;pagescroll.left();break;
		case 'credit3':hideMenu();$('moreconf').style.display = 'none';$('extcreditbox3').innerHTML = $('extcreditbox').innerHTML;pagescroll.left();break;
		case 'return':if(!Editorwin) {hideMenu();$('custominfoarea').style.display=$('more_2').style.display='none';pagescroll.up(1, '$(\'more_1\').style.display=\'\'');}break;
		case 'creditreturn':pagescroll.right(1, '$(\'moreconf\').style.display = \'\';');break;
		case 'swf':hideMenu();$('moreconf').style.display = 'none';swfHandler(3);break;
		case 'swfreturn':$('swfbox').style.display = 'none';if(!Editorwin) {pagescroll.left(1, '$(\'moreconf\').style.display = \'\';');}swfHandler(2);break;
		case 'more':hideMenu();pagescroll.down(1, '$(\'more_1\').style.display=$(\'more_2\').style.display=$(\'custominfoarea\').style.display=\'none\'');break;
		case 'editorreturn':$('more_1').style.display='none';pagescroll.up(1, '$(\'more_2\').style.display=$(\'custominfoarea\').style.display=\'\'');break;
		case 'editor':$('more_1').style.display='none';pagescroll.down(1, '$(\'more_2\').style.display=\'\';$(\'custominfoarea\').style.display=\'\'');break;
	}
}

function switchicon(iconid, obj) {
	$('iconid').value = iconid;
	$('icon_img').src = obj.src;
	hideMenu();
}

var swfuploaded = 0;
function swfHandler(action) {
	if(action == 1) {
		swfuploaded = 1;
	} else if(action == 2) {
		if(Editorwin || !infloat) {
			swfuploadwin();
		} else {
			$('swfbox').style.display = 'none';
			pagescroll.left(1, '$(\'moreconf\').style.display=\'\';');
		}
		if(swfuploaded) {
			swfattachlistupdate(action);
		}
	} else if(action == 3) {
		swfuploaded = 0;
		pagescroll.right(1, '$(\'swfuploadbox\').style.display = $(\'swfbox\').style.display = \'\';');
	}
}

function swfattachlistupdate(action) {
	ajaxget('ajax.php?action=swfattachlist', 'swfattachlist', 'swfattachlist', 'swfattachlist', null, '$(\'uploadlist\').scrollTop=10000');
	attachlist('open');
	$('postform').updateswfattach.value = 1;
}

function appendreply() {
	newpos = fetchOffset($('post_new'));
	document.documentElement.scrollTop = newpos['top'];
	$('post_new').style.display = '';
	$('post_new').id = '';
	div = document.createElement('div');
	div.id = 'post_new';
	div.style.display = 'none';
	div.className = '';
	$('postlistreply').appendChild(div);
	$('postform').replysubmit.disabled = false;
	creditnoticewin();
}

var Editorwin = 0;
function resizeEditorwin() {
	var obj = $('resizeEditorwin');
	floatwin('size_' + editoraction);
	$('editorbox').style.height = $('floatlayout_' + editoraction).style.height = $('floatwin_' + editoraction).style.height;
	if(!Editorwin) {
		obj.className = 'float_min';
		obj.title = obj.innerHTML = '还原大小';
		$('editorbox').style.width = $('floatlayout_' + editoraction).style.width = (parseInt($('floatwin_' + editoraction).style.width) - 10)+ 'px';
		$('editorbox').style.left = '0px';
		$('editorbox').style.top = '0px';
		$('swfuploadbox').style.display = $('custominfoarea').style.display = $('creditlink').style.display = $('morelink').style.display = 'none';
		if(wysiwyg) {
			$('e_iframe').style.height = (parseInt($('floatwin_' + editoraction).style.height) - 150)+ 'px';
		}
		$('e_textarea').style.height = (parseInt($('floatwin_' + editoraction).style.height) - 150)+ 'px';
		attachlist('close');
		Editorwin = 1;
	} else {
		obj.className = 'float_max';
		obj.title = obj.innerHTML = '最大化';
		$('editorbox').style.width = $('floatlayout_' + editoraction).style.width = '600px';
		$('swfuploadbox').style.display = $('custominfoarea').style.display = $('creditlink').style.display = $('morelink').style.display = '';
		if(wysiwyg) {
			$('e_iframe').style.height = '';
		}
		$('e_textarea').style.height = '';
		swfuploadwin();
		Editorwin = 0;
	}
}

function closeEditorwin() {
	if(Editorwin) {
		resizeEditorwin();
	}
	floatwin('close_' + editoraction);
}

function editorwindowopen(url) {
	data = wysiwyg ? editdoc.body.innerHTML : textobj.value;
	saveData(data);
	url += '&cedit=' + (data !== '' ? 'yes' : 'no');
	window.open(url);
}

function swfuploadwin() {
	if(Editorwin) {
		if($('swfuploadbox').style.display == 'none') {
			$('swfuploadbox').className = 'floatbox floatbox1 floatboxswf floatwin swfwin';
			$('swfuploadbox').style.position = 'absolute';
			width = (parseInt($('floatlayout_' + editoraction).style.width) - 604) / 2;
			$('swfuploadbox').style.left = width + 'px';
			$('swfuploadbox').style.display = $('swfclosebtn').style.display = $('swfbox').style.display = '';

		} else {
			$('swfuploadbox').className = 'floatbox floatbox1 floatboxswf';
			$('swfuploadbox').style.position = $('swfuploadbox').style.left = '';
			$('swfuploadbox').style.display = $('swfclosebtn').style.display = 'none';
		}
	} else {
		if(infloat) {
			pagescrolls('swf');
		} else {
			if($('swfuploadbox').style.display == 'none') {
				$('swfuploadbox').style.display = $('swfbox').style.display = $('swfclosebtn').style.display = '';
			} else {
				$('swfuploadbox').style.display = $('swfbox').style.display = $('swfclosebtn').style.display = 'none';
			}
		}
	}
}


function uploadAttach(curId, statusid, prefix) {
	prefix = isUndefined(prefix) ? '' : prefix;
	var nextId = 0;
	for(var i = 0; i < AID - 1; i++) {
		if($(prefix + 'attachform_' + i)) {
			nextId = i;
			if(curId == 0) {
				break;
			} else {
				if(i > curId) {
					break;
				}
			}
		}
	}
	if(nextId == 0) {
		return;
	}
	CURRENTATTACH = nextId + '|' + prefix;
	if(curId > 0) {
		if(statusid == 0) {
			UPLOADCOMPLETE++;
		} else {
			FAILEDATTACHS += '<br />' + mb_cutstr($(prefix + 'attachnew_' + curId).value.substr($(prefix + 'attachnew_' + curId).value.replace(/\\/g, '/').lastIndexOf('/') + 1), 25) + ': ' + STATUSMSG[statusid];
			UPLOADFAILED++;
		}
		$(prefix + 'cpdel_' + curId).innerHTML = '<img src="' + IMGDIR + '/check_' + (statusid == 0 ? 'right' : 'error') + '.gif" alt="' + STATUSMSG[statusid] + '" />';
		if(nextId == curId || in_array(statusid, [6, 8])) {
			if(prefix == 'img') {
			updateImageList();
			}
			else 
			{
			updateAttachListbycount(UPLOADCOMPLETE);
			}
			if(UPLOADFAILED > 0) {
				showDialog('附件上传完成！成功 ' + UPLOADCOMPLETE + ' 个，失败 ' + UPLOADFAILED + ' 个:' + FAILEDATTACHS);
				FAILEDATTACHS = '';
			}
			UPLOADSTATUS = 2;
			for(var i = 0; i < AID - 1; i++) {
				if($(prefix + 'attachform_' + i)) {
					reAddAttach(prefix, i)
				}
			}
			$(prefix + 'uploadbtn').style.display = '';
			$(prefix + 'uploading').style.display = 'none';
			if(AUTOPOST) {
				hideMenu();
				validate($('postform'));
			} else if(UPLOADFAILED == 0 && (prefix == 'img' || prefix == '')) {
				showDialog('附件上传完成！', 'notice');
			}
			UPLOADFAILED = UPLOADCOMPLETE = 0;
			CURRENTATTACH = '0';
			FAILEDATTACHS = '';
			return;
		}
	} else {
		$(prefix + 'uploadbtn').style.display = 'none';
		$(prefix + 'uploading').style.display = '';
	}
	$(prefix + 'cpdel_' + nextId).innerHTML = '<img src="' + IMGDIR + '/loading.gif" alt="上传中..." />';
	UPLOADSTATUS = 1;
	$(prefix + 'attachform_' + nextId).submit();
}

var postSubmited = false;
var AID = 1;
var UPLOADSTATUS = -1;
var UPLOADFAILED = UPLOADCOMPLETE = AUTOPOST =  0;
var CURRENTATTACH = '0';
var FAILEDATTACHS = '';
var UPLOADWINRECALL = null;
var STATUSMSG = {'-1' : '内部服务器错误', '0' : '上传成功', '1' : '不支持此类扩展名', '2' : '附件大小为 0', '3' : '附件大小超限', '4' : '不支持此类扩展名', '5' : '附件大小超限', '6' : '附件总大小超限', '7' : '图片附件不合法', '8' : '附件文件无法保存', '9' : '没有合法的文件被上传', '10' : '非法操作','11' : '您没有上传权限'};

function checkFocus() {
	var obj = wysiwyg ? editwin : textobj;
	if(!obj.hasfocus) {
		obj.focus();
	}
}

function ctlent(event) {
	if(postSubmited == false && (event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83) && $('postsubmit')) {
		if(in_array($('postsubmit').name, ['topicsubmit', 'replysubmit', 'editsubmit']) && !validate($('postform'))) {
			doane(event);
			return;
		}
		postSubmited = true;
		$('postsubmit').disabled = true;
		$('postform').submit();
	}
	if(event.keyCode == 9) {
		doane(event);
	}
}

function checklength(theform) {
	var message = wysiwyg ? html2bbcode(getEditorContents()) : (!theform.parseurloff.checked ? parseurl(theform.message.value) : theform.message.value);
	showDialog('当前长度: ' + mb_strlen(message) + ' 字节，' + (postmaxchars != 0 ? '系统限制: ' + postminchars + ' 到 ' + postmaxchars + ' 字节。' : ''), 'notice', '字数检查');
}

if(!tradepost) {
	var tradepost = 0;
}

/* function validatedddddddddddddddddddd(theform) {
	var message = wysiwyg ? html2bbcode(getEditorContents()) : (!theform.parseurloff.checked ? parseurl(theform.message.value) : theform.message.value);
	if(($('postsubmit').name != 'replysubmit' && !($('postsubmit').name == 'editsubmit' && !isfirstpost) && theform.subject.value == "") || !sortid && !special && trim(message) == "") {
		showDialog('请完成标题或内容栏');
		return false;
	} else if(mb_strlen(theform.subject.value) > 80) {
		showDialog('您的标题超过 80 个字符的限制');
		return false;
	}
	if(in_array($('postsubmit').name, ['topicsubmit', 'editsubmit'])) {
		if(theform.typeid && (theform.typeid.options && theform.typeid.options[theform.typeid.selectedIndex].value == 0) && typerequired) {
			showDialog('请选择主题对应的分类');
			return false;
		}
		if(theform.sortid && (theform.sortid.options && theform.sortid.options[theform.sortid.selectedIndex].value == 0) && sortrequired) {
			showDialog('请选择主题对应的分类信息');
			return false;
		}
	}
	if(typeof validateextra == 'function') {
		var v = validateextra();
		if(!v) {
			return false;
		}
	}

	if(!disablepostctrl && !sortid && !special && ((postminchars != 0 && mb_strlen(message) < postminchars) || (postmaxchars != 0 && mb_strlen(message) > postmaxchars))) {
		showDialog('您的帖子长度不符合要求。\n\n当前长度: ' + mb_strlen(message) + ' 字节\n系统限制: ' + postminchars + ' 到 ' + postmaxchars + ' 字节');
		return false;
	}
	if(UPLOADSTATUS == 0) {
		if(!confirm('您有等待上传的附件，确认不上传这些附件吗？')) {
			return false;
		}
	} else if(UPLOADSTATUS == 1) {
		showDialog('您有正在上传的附件，请稍候，上传完成后帖子将会自动发表...', 'notice');
		AUTOPOST = 1;
		return false;
	}
	if($(editorid + '_attachlist')) {
		$('postbox').appendChild($(editorid + '_attachlist'));
		$(editorid + '_attachlist').style.display = 'none';
	}
	if($(editorid + '_imgattachlist')) {
		$('postbox').appendChild($(editorid + '_imgattachlist'));
		$(editorid + '_imgattachlist').style.display = 'none';
	}
	hideMenu();
	theform.message.value = message;
	if($('postsubmit').name == 'editsubmit') {
		return true;
	} else if(in_array($('postsubmit').name, ['topicsubmit', 'replysubmit'])) {
		if(seccodecheck || secqaacheck) {
			var chk = 1;
			if(secqaacheck && $('checksecqaaverify_' + theform.sechash.value).innerHTML.indexOf('check_right') == -1) {
				showDialog('验证问答错误，请重新填写');
				chk = 0;
			}
			if(seccodecheck && $('checkseccodeverify_' + theform.sechash.value).innerHTML.indexOf('check_right') == -1) {
				showDialog('验证码错误，请重新填写');
				chk = 0;
			}
			if(chk) {
				postsubmit(theform);
			}
		} else {
			postsubmit(theform);
		}
		return false;
	}
}
 */
function postsubmit(theform) {
	theform.replysubmit ? theform.replysubmit.disabled = true : (theform.editsubmit ? theform.editsubmit.disabled = true : theform.topicsubmit.disabled = true);
	theform.submit();
}

 function loadData(quiet) {
	var data = '';
	data = loadUserdata('forum');
	
	if(in_array((data = trim(data)), ['', 'null', 'false', null, false])) {
		if(!quiet) {
			showDialog('没有可以恢复的数据！');
		}
		return;
	}

	if(!quiet && !confirm('此操作将覆盖当前帖子内容，确定要恢复数据吗？')) {
		return;
	}

	var data = data.split(/\x09\x09/);
	for(var i = 0; i < $('postform').elements.length; i++) {
		var el = $('postform').elements[i];
		if(el.name != '' && (el.tagName == 'TEXTAREA' || el.tagName == 'INPUT' && (el.type == 'text' || el.type == 'checkbox' || el.type == 'radio'))) {
			for(var j = 0; j < data.length; j++) {
				var ele = data[j].split(/\x09/);
				if(ele[0] == el.name) {
					elvalue = !isUndefined(ele[3]) ? ele[3] : '';
					if(ele[1] == 'INPUT') {
						if(ele[2] == 'text') {
							el.value = elvalue;
						} else if((ele[2] == 'checkbox' || ele[2] == 'radio') && ele[3] == el.value) {
							el.checked = true;
							evalevent(el);
						}
					} else if(ele[1] == 'TEXTAREA') {
						if(ele[0] == 'message') {
							if(!wysiwyg) {
								textobj.value = elvalue;
							} else {
								editdoc.body.innerHTML = bbcode2html(elvalue);
							}
						} else {
							el.value = elvalue;
						}
					}
					break
				}
			}
		}
	}
}

function evalevent(obj) {
	var script = obj.parentNode.innerHTML;
	var re = /onclick="(.+?)["|>]/ig;
	var matches = re.exec(script);
	if(matches != null) {
		matches[1] = matches[1].replace(/this\./ig, 'obj.');
		eval(matches[1]);
	}
}

function relatekw(subject, message, recall) {
	if(isUndefined(recall)) recall = '';
	if(isUndefined(subject) || subject == -1) subject = $('subject').value;
	if(isUndefined(message) || message == -1) message = getEditorContents();
	subject = (BROWSER.ie && document.charset == 'utf-8' ? encodeURIComponent(subject) : subject);
	message = (BROWSER.ie && document.charset == 'utf-8' ? encodeURIComponent(message) : message);
	message = message.replace(/&/ig, '', message).substr(0, 500);
	ajaxget('forum.php?mod=relatekw&subjectenc=' + subject + '&messageenc=' + message, 'tagselect', '', '', '', recall);
}

function switchicon(iconid, obj) {
	$('iconid').value = iconid;
	$('icon_img').src = obj.src;
	hideMenu();
}

function clearContent() {
	if(wysiwyg) {
		editdoc.body.innerHTML = BROWSER.firefox ? '<br />' : '';
	} else {
		textobj.value = '';
	}
}

function uploadNextAttach() {
	var str = $('attachframe').contentWindow.document.body.innerHTML;
	if(str == '') return;
	var arr = str.split('|');
	var att = CURRENTATTACH.split('|');
	uploadAttach(parseInt(att[0]), arr[0] == 'DISCUZUPLOAD' ? parseInt(arr[1]) : -1, att[1]);
	if (arr[0] == "DISCUZUPDATE") {
	    if (arr[1] == "0") {
	        $("attachname" + arr[3]).innerHTML = arr[2];
	        $("attach" + arr[3]).style.display = '';
	        $("attachupdate" + arr[3]).style.display = 'none';
	        $("attach" + arr[3] + "_opt").innerHTML = '更新';
	        $("attach" + arr[3] + "_type").src = arr[4] == -1 ? "images/attachicons/attachment.gif" : "images/attachicons/image.gif";
	    }
	    else {
	        showDialog('更新失败:' + STATUSMSG[arr[1]]);
	    }
	}
}


function addAttach(prefix) {
	var id = AID;
	var tags, newnode, i;
	prefix = isUndefined(prefix) ? '' : prefix;
	newnode = $(prefix + 'attachbtnhidden').firstChild.cloneNode(true);
	tags = newnode.getElementsByTagName('input');
	for(i in tags) {
		if(tags[i].name == 'Filedata') {
			tags[i].id = prefix + 'attachnew_' + id;
			tags[i].onchange = function() {insertAttach(prefix, id)};
			tags[i].unselectable = 'on';
		} else if(tags[i].name == 'attachid') {
			tags[i].value = id;
		}
	}
	tags = newnode.getElementsByTagName('form');
	tags[0].name = tags[0].id = prefix + 'attachform_' + id;
	$(prefix + 'attachbtn').appendChild(newnode);
	newnode = $(prefix + 'attachbodyhidden').firstChild.cloneNode(true);
	tags = newnode.getElementsByTagName('input');
	for(i in tags) {
		if(tags[i].name == prefix + 'localid') {
			tags[i].value = id;
		}
	}
	tags = newnode.getElementsByTagName('span');
	for(i in tags) {
		if(tags[i].id == prefix + 'localfile[]') {
			tags[i].id = prefix + 'localfile_' + id;
		} else if(tags[i].id == prefix + 'cpdel[]') {
			tags[i].id = prefix + 'cpdel_' + id;
		} else if(tags[i].id == prefix + 'localno[]') {
			tags[i].id = prefix + 'localno_' + id;
		} else if(tags[i].id == prefix + 'deschidden[]') {
			tags[i].id = prefix + 'deschidden_' + id;
		}
	}
	AID++;
	newnode.style.display = 'none';
	$(prefix + 'attachbody').appendChild(newnode);
}

function insertAttach(prefix, id) {
	var localimgpreview = '';
	var path = $(prefix + 'attachnew_' + id).value;
	var extpos = path.lastIndexOf('.');
	var ext = extpos == -1 ? '' : path.substr(extpos + 1, path.length).toLowerCase();
	var re = new RegExp("(^|\\s|,)" + ext + "($|\\s|,)", "ig");
	var localfile = $(prefix + 'attachnew_' + id).value.substr($(prefix + 'attachnew_' + id).value.replace(/\\/g, '/').lastIndexOf('/') + 1);
	var filename = mb_cutstr(localfile, 30);

	if(path == '') {
		return;
	}
	var uplaodextensions=new Array();
	var extensionsarray=extensions.split('|')
	for(i=0;i<extensionsarray.length;i++)
	{
	    var t=extensionsarray[i].split(',')
	    uplaodextensions.push(t[0]);
	}
	//if(extensions != '' && (re.exec(extensions) == null || ext == '')) {
	if(uplaodextensions != '' && (re.exec(uplaodextensions) == null || ext == '')) {
		//reAddAttach(prefix, id);
		showDialog('对不起，不支持上传此类扩展名的附件');
		return;
	}
	if(prefix == 'img' && imgexts.indexOf(ext) == -1) {
		reAddAttach(prefix, id);
		showDialog('请选择图片文件(' + imgexts + ')');
		return;
	}

    $(prefix + 'cpdel_' + id).innerHTML = '<a href="###" class="d" onclick="reAddAttach(\'' + prefix + '\', ' + id + ')" title="删除">删除</a>';
	$(prefix + 'localfile_' + id).innerHTML = '<span>' + filename + '</span>';
	$(prefix + 'attachnew_' + id).style.display = 'none';
	$(prefix + 'deschidden_' + id).style.display = '';
	$(prefix + 'deschidden_' + id).title = localfile;
	$(prefix + 'localno_' + id).parentNode.parentNode.style.display = '';
	addAttach(prefix);
	UPLOADSTATUS = 0;
}

function reAddAttach(prefix, id) {
	$(prefix + 'attachbody').removeChild($(prefix + 'localno_' + id).parentNode.parentNode);
	$(prefix + 'attachbtn').removeChild($(prefix + 'attachnew_' + id).parentNode.parentNode);
	$(prefix + 'attachbody').innerHTML == '' && addAttach(prefix);
	$('localimgpreview_' + id) ? document.body.removeChild($('localimgpreview_' + id)) : null;
}

function delAttach(id, type) {
	//appendAttachDel(id);
	//$('attach_' + id).style.display = 'none';
	//ATTACHNUM['attach' + (type ? 'un' : '') + 'used']--;
	//updateattachnum('attach');
	data=id.toString().split(",")
	_sendRequest('tools/ajax.aspx?t=deletenouseattach&aid='+data[0]+'&fid='+data[1], delnouseAttach_callback, false);
}

function delnouseAttach_callback(doc)
{

	var data=eval(doc)
	if(data.length!=0)
	{
	    appendAttachDel(data[0].aid);
	    if ($('attach_' + data[0].aid))
	        $('attach_' + data[0].aid).parentNode.removeChild($('attach_' + data[0].aid));
		//ATTACHNUM['attach' + (type ? 'un' : '') + 'used']--;
		updateattachnum('attach');
	//updateAttachList();
	}
}
function delImgAttach(id, type) {
	appendAttachDel(id);
	$('image_td_' + id).className = 'imgdeleted';
	$('image_' + id).onclick = null;
	$('image_desc_' + id).disabled = true;
	delAttach(id, type)
//	ATTACHNUM['image' + (type ? 'un' : '') + 'used']--;
//	updateattachnum('image');
}

function appendAttachDel(id) {
	var input = document.createElement('input');
	input.type = 'hidden';
	input.name = 'attachdel[]';
	input.value = id;
	$('postbox').appendChild(input);
}

function updateAttach(aid) {
	objupdate = $('attachupdate'+aid);
	obj = $('attach' + aid);
	if(!objupdate.innerHTML) {
		obj.style.display = 'none';
		objupdate.innerHTML = '<input type="file" name="attachupdate[paid' + aid + ']"><a href="javascript:;" onclick="updateAttach(' + aid + ')">取消</a>';
	} else {
		obj.style.display = '';
		objupdate.innerHTML = '';
	}
}

function updateattachnum(type) {
	ATTACHNUM[type + 'used'] = ATTACHNUM[type + 'used'] >= 0 ? ATTACHNUM[type + 'used'] : 0;
	ATTACHNUM[type + 'unused'] = ATTACHNUM[type + 'unused'] >= 0 ? ATTACHNUM[type + 'unused'] : 0;
	var num = ATTACHNUM[type + 'used'] + ATTACHNUM[type + 'unused'];
	if(num) {
		if($(editorid + '_' + type)) {
			$(editorid + '_' + type).title = '包含 ' + num + (type == 'image' ? ' 个图片附件' : ' 个附件');
		}
		if($(editorid + '_' + type + 'n')) {
			$(editorid + '_' + type + 'n').style.display = '';
		}
	} else {
		if($(editorid + '_' + type)) {
			$(editorid + '_' + type).title = type == 'image' ? '图片' : '附件';
		}
		if($(editorid + '_' + type + 'n')) {
			$(editorid + '_' + type + 'n').style.display = 'none';
		}
	}
}

/* function unusedoption(op, aid) {
	if(!op) {
		if($('unusedimgattachlist')) {
			$('unusedimgattachlist').parentNode.removeChild($('unusedimgattachlist'));
		}
		if($('unusedattachlist')) {
			$('unusedattachlist').parentNode.removeChild($('unusedattachlist'));
		}
		ATTACHNUM['imageunused'] = 0;
		ATTACHNUM['attachunused'] = 0;
	} else if(op == 1) {
		for(var i = 0; i < $('unusedform').elements.length; i++) {
			var e = $('unusedform').elements[i];
			if(e.name.match('unused')) {
				if(!e.checked) {
					if($('image_td_' + e.value)) {
						$('image_td_' + e.value).parentNode.removeChild($('image_td_' + e.value));
						ATTACHNUM['imageunused']--;
					}
					if($('attach_' + e.value)) {
						$('attach_' + e.value).parentNode.removeChild($('attach_' + e.value));
						ATTACHNUM['attachunused']--;
					}
				}
			}
		}
	} else if(op == 2) {
		delAttach(aid, 1);
	} else if(op == 3) {
		delImgAttach(aid, 1);
	}
	if(op < 2) {
		hideMenu('fwin_dialog', 'dialog');
		updateattachnum('image');
		updateattachnum('attach');
	} else {
		$('unusedrow' + aid).outerHTML = '';
		if(!ATTACHNUM['imageunused'] && !ATTACHNUM['attachunused']) {
			hideMenu('fwin_dialog', 'dialog');
		}
	}
} */
function unusedoption(op, aid) {
	if(!op) {
/* 		if($('unusedimgattachlist')) {
			$('unusedimgattachlist').parentNode.removeChild($('unusedimgattachlist'));
		}

		if($('unusedattachlist')) {
			$('unusedattachlist').parentNode.removeChild($('unusedattachlist'));
		}

		ATTACHNUM['imageunused'] = 0;
		ATTACHNUM['attachunused'] = 0; */
		for(var i = 0; i < $('unusedform').elements.length; i++) {
			var e = $('unusedform').elements[i];
			if(e.name.match('unused')) {
				if(e.checked) {
					if($('image_td_' + e.value)) {
						$('image_td_' + e.value).parentNode.removeChild($('image_td_' + e.value));
						ATTACHNUM['imageunused']--;
					}
					if($('attach_' + e.value)) {
						$('attach_' + e.value).parentNode.removeChild($('attach_' + e.value));
						ATTACHNUM['attachunused']--;
					}
				}
			}
		}
	} else if(op == 1) {
		for(var i = 0; i < $('unusedform').elements.length; i++) {
			var e = $('unusedform').elements[i];
			if(e.name.match('unused')) {
				if(!e.checked) {
					if($('image_td_' + e.value)) {
						$('image_td_' + e.value).parentNode.removeChild($('image_td_' + e.value));
						ATTACHNUM['imageunused']--;
					}
					if($('attach_' + e.value)) {
						$('attach_' + e.value).parentNode.removeChild($('attach_' + e.value));
						ATTACHNUM['attachunused']--;
					}
				}
			}
		}
	} else if(op == 2) {
		delAttach(aid, 1);
	} else if(op == 3) {
		delImgAttach(aid, 1);
	}
	if(op < 2) {
		hideMenu('fwin_dialog', 'dialog');
		updateattachnum('image');
		updateattachnum('attach');
	} else {
		$('unusedrow' + aid).outerHTML = '';
		if(!ATTACHNUM['imageunused'] && !ATTACHNUM['attachunused']) {
			hideMenu('fwin_dialog', 'dialog');
		}
	}
}
function swfHandler(action, type) {
	if(action == 2) {
		if(type == 'image') {
			updateImageList(action);
		} else {
			updateAttachList(action);
		}
	}
}

function getfileextname(filename)
{
   if(filename.indexOf('.')!=-1)
   {
     var arr=filename.split('.');
	 return arr[arr.length-1];
   }
}

function getattachlist_callback(doc) {
    var data = eval(doc);
    html = '';
    hiddenattachidlist = '';
    if (data.length > 0) {
        html += '<table cellpadding="0" cellspacing="0" border="0" width="100%">';
        html += '<tbody><tr><td colspan="6">以下是你上次上传但没有使用的附件:</td></tr></tbody>'
        for (i = 0; i < data.length; i++) {
            linkurl = '';
            isimage = '';
            filetypeimage = '';
            html += '<tr>';
            hiddenattachidlist += data[i].aid + ',';
            fileext = getfileextname(data[i].attachment);

            var isimgbool = data[i].filetype.indexOf('image') != -1;

            if (isimgbool) {
                linkurl = 'insertAttachimgTag(' + data[i].aid + ')';
                isimage = ' isimage="1"';
                filetypeimage = 'image.gif';
            }
            else {
                linkurl = 'insertAttachTag(' + data[i].aid + ')';
                if (fileext == 'rar' || fileext == 'zip')
                    filetypeimage = 'rar.gif';
                else
                    filetypeimage = 'attachment.gif';
            }

//            if (data[i].filetype.indexOf('image') != -1) {
//                linkurl = 'insertAttachimgTag(' + data[i].aid + ')'
//                isimage = ' isimage="1"';
//            }
//            else
//            { linkurl = 'insertAttachTag(' + data[i].aid + ')' }

//            if (data[i].filetype.indexOf('image') != -1)
//            { filetypeimage = 'image.gif' }
//            else {
//                if (fileext == 'rar' || fileext == 'zip')
//                { filetypeimage = 'rar.gif' }
//                else
//                { filetypeimage = 'attachment.gif' }
//            }

            html += '<tbody id="attach_' + data[i].aid + '"><tr><td class="atnu">';
            html += '<img border="0" alt="" class="vm" src="images/attachicons/' + filetypeimage + '">';
            html += '</td><td class="atna">';
            html += '<a id="attachname' + data[i].aid + '" href="javascript:;" onclick="' + linkurl + '" ' + isimage + ' title="' + data[i].attachment + '">' + mb_cutstr(data[i].attachment, 40) + '</a>';
            html += '<span id="attachupdate' + data[i].aid + '"></span>';
            html += '<input type="hidden" name="attachid" value="' + data[i].aid + '">';
            if (isimgbool)
                html += '<img style="position: absolute; top: -10000px;" cwidth="' + data[i].height + '" id="image_' + data[i].aid + '" src="' + forumpath + 'attachment.aspx?attachmentid=' + data[i].aid + '"/>';
            html += '</td>';
            html += '<td class="atds"><input type="text" value="" class="txt" size="18" name="attachdesc_' + data[i].aid + '"></td>';
            html += '<td class="attv"><input type="text" name="readperm_' + data[i].aid + '" value="0" size="1" class="txt"></td>';
            html += '<td class="attp"><input type="text" name="attachprice_' + data[i].aid + '" value="0" size="1" class="txt"></td>';
            html += '<td class="attc delete_msg"><a onclick="delAttach(' + data[i].aid + ',1)" class="d" href="javascript:;" title="删除">删除</a></td>';
            html += '</tr></tbody>';
        }
        html += '</table>';
        $("attachlist_tablist").innerHTML = html;
    }
}

function updateAttachListbycount(attachlistcount) {
    var url = 'tools/ajax.aspx?t=getattachlist&posttime=' + encodeURI($("posttime").value);
	_sendRequest(url, updateAttachListbycount_callback, false);
	switchAttachbutton('attachlist');
}

function updateAttachListbycount_callback(doc) {
    var data = eval(doc);
    html = '';
    hiddenattachidlist = '';
    if (data.length > 0) {
        html += '<table cellpadding="0" cellspacing="0" border="0" width="100%">';
        for (i = 0; i < data.length; i++) {
            //如果上次上传但没有使用的附件列表中已经存在该附件条目，则不重复添加.如果该位置的html改变了层级结构，可能会造成第二个判断无法生效
            if ($('attach_' + data[i].aid) != null && $('attach_' + data[i].aid).parentNode.parentNode.id == 'attachlist_tablist')
                continue;
            linkurl = '';
            isimage = ''
            filetypeimage = '';
            html += '<tr>';
            hiddenattachidlist += data[i].aid + ',';
            fileext = getfileextname(data[i].attachment);

            var isimgbool = data[i].filetype.indexOf('image') != -1;

            if (isimgbool) {
                linkurl = 'insertAttachimgTag(' + data[i].aid + ')';
                isimage = ' isimage="1"';
                filetypeimage = 'image.gif';
            }
            else {
                linkurl = 'insertAttachTag(' + data[i].aid + ')';
                if (fileext == 'rar' || fileext == 'zip')
                    filetypeimage = 'rar.gif';
                else
                    filetypeimage = 'attachment.gif';
            }

//            if (data[i].filetype.indexOf('image') != -1) {
//                linkurl = 'insertAttachimgTag(' + data[i].aid + ')';
//                isimage = ' isimage="1"';
//            }

//            else
//                linkurl = 'insertAttachTag(' + data[i].aid + ')';

//            if (data[i].filetype.indexOf('image') != -1)
//                filetypeimage = 'image.gif';
//            else {
//                if (fileext == 'rar' || fileext == 'zip')
//                    filetypeimage = 'rar.gif';
//                else
//                    filetypeimage = 'attachment.gif';
//            }

            html += '<tbody id="attach_' + data[i].aid + '"><tr><td class="atnu">';
            html += '<img border="0" alt="" class="vm" src="images/attachicons/' + filetypeimage + '">';
            html += '</td><td class="atna">';
            html += '<a id="attachname' + data[i].aid + '" href="javascript:;" onclick="' + linkurl + '" ' + isimage + ' title="' + data[i].attachment + '">' + mb_cutstr(data[i].attachment, 40) + '</a>';
            html += '<span id="attachupdate' + data[i].aid + '"></span>';
            html += '<input type="hidden" name="attachid" value="' + data[i].aid + '">';
            if (isimgbool)
                html += '<img style="position: absolute; top: -10000px;" cwidth="' + data[i].height + '" id="image_' + data[i].aid + '" src="' + forumpath + 'attachment.aspx?attachmentid=' + data[i].aid + '"/>';
            html += '</td>';
            html += '<td class="atds"><input type="text" value="" class="txt" size="18" name="attachdesc_' + data[i].aid + '"></td>';
            html += '<td class="attv"><input type="text" name="readperm_' + data[i].aid + '" value="0" class="txt"  size="1"></td>';
            html += '<td class="attp"><input type="text" name="attachprice_' + data[i].aid + '" value="0" class="txt"  size="1"></td>';
            html += '<td class="attc delete_msg"><a onclick="delAttach(' + data[i].aid + ',1)" class="d" href="javascript:;" title="删除">删除</a></td>';
            html += '</tr></tbody>';
        }
        html += '</table>';
        $('attachlist_tablist_current').innerHTML = html;
    }
}

function updateAttachList(action) {
    //alert("updateAttachList:" + action);
    var url = 'tools/ajax.aspx?t=getattachlist' + (action ? "&posttime=" + encodeURI($("posttime").value) : "") + "&file=true";
    var fun = action ? updateAttachListbycount_callback : getattachlist_callback;
	_sendRequest(url, fun, false);
	switchAttachbutton('attachlist');
	$('attachlist_tablist').style.display = '';
	$('attach_notice').style.display = '';
}

function updateImageList(action) {
    //alert("updateImageList:" + action);
    var url = 'tools/ajax.aspx?t=imagelist&pid=' + pid + (action ? '&posttime=' + encodeURI($("posttime").value) : "") + (!fid ? '' : '&fid=' + fid);
    if (action)
        ajaxget(url, 'imgattachlist');
    else
        ajaxget(url, 'unusedimgattachlist');
	switchImagebutton('imgattachlist');$('imgattach_notice').style.display = '';
}

function switchButton(btn, btns) {
	if(!$(editorid + '_btn_' + btn) || !$(editorid + '_' + btn)) {
		return;
	}
	$(editorid + '_btn_' + btn).style.display = '';
	$(editorid + '_' + btn).style.display = '';
	$(editorid + '_btn_' + btn).className = 'current';
	for(i = 0;i < btns.length;i++) {
		if(btns[i] != btn) {
			if(!$(editorid + '_' + btns[i]) || !$(editorid + '_btn_' + btns[i])) {
				continue;
			}
			$(editorid + '_' + btns[i]).style.display = 'none';
			$(editorid + '_btn_' + btns[i]).className = '';
		}
	}
}

function uploadWindowstart() {
	$('uploadwindowing').style.visibility = 'visible';
	$('uploadsubmit').disabled = true;
}

function uploadWindowload() {
	$('uploadwindowing').style.visibility = 'hidden';
	$('uploadsubmit').disabled = false;
	var str = $('uploadattachframe').contentWindow.document.body.innerHTML;
	if(str == '') return;
	var arr = str.split('|');
	if(arr[0] == 'DISCUZUPLOAD' && arr[2] == 0) {
		UPLOADWINRECALL(arr[3], arr[5], arr[6]);
		hideWindow('upload');
	} else {
		showDialog('上传失败:' + STATUSMSG[arr[2]]);
	}
}

function uploadWindow(recall, type) {
	var type = isUndefined(type) ? 'image' : type;
	UPLOADWINRECALL = recall;
	showWindow('upload', 'forum.php?mod=misc&action=upload&fid=' + fid + '&type=' + type, 'get', 0, {'cover':1});
}

function updatetradeattach(aid, url, attachurl) {
	$('tradeaid').value = aid;
	$('tradeattach_image').innerHTML = '<img src="' + attachurl + '/' + url + '" class="spimg" />';
}

function updateactivityattach(aid, url, attachurl) {
	$('activityaid').value = aid;
	$('activityattach_image').innerHTML = '<img src="' + attachurl + '/' + url + '" class="spimg" />';
}

function updatesortattach(aid, url, attachurl) {
	$('sortaid').value = aid;
	$('sortattachurl').value = attachurl + '/' + url;
	$('sortattach_image').innerHTML = '<img src="' + attachurl + '/' + url + '" class="spimg" />';
}

function switchpollm(swt) {
	t = $('pollchecked').checked && swt ? 2 : 1;
	var v = '';
	for(var i = 0; i < $('postform').elements.length; i++) {
		var e = $('postform').elements[i];
		if(e.name.match('^polloption')) {
			if(t == 2 && e.tagName == 'INPUT') {
				v += e.value + '\n';
			} else if(t == 1 && e.tagName == 'TEXTAREA') {
				v += e.value;
			}
		}
	}
	if(t == 1) {
		var a = v.split('\n');
		var pcount = 0;
		for(var i = 0; i < $('postform').elements.length; i++) {
			var e = $('postform').elements[i];
			if(e.name.match('^polloption')) {
				pcount++;
				if(e.tagName == 'INPUT') e.value = '';
			}
		}
		for(var i = 0; i < a.length - pcount + 2; i++) {
			addpolloption();
		}
		var ii = 0;
		for(var i = 0; i < $('postform').elements.length; i++) {
			var e = $('postform').elements[i];
			if(e.name.match('^polloption') && e.tagName == 'INPUT' && a[ii]) {
				e.value = a[ii++];
			}
		}
	} else if(t == 2) {
		$('postform').polloptions.value = trim(v);

	}
	$('postform').tpolloption.value = t;
	if(swt) {
		display('pollm_c_1');
		display('pollm_c_2');
	}
}

function loadimgsize(imgurl) {
	var s = new Object();
	s.img = new Image();
	s.img.src = imgurl;
	s.loadCheck = function () {
		if(s.img.complete) {
			$(editorid + '_image_submit').disabled = false;
			$(editorid + '_image_param_2').value = s.img.width ? s.img.width : '';
			$(editorid + '_image_param_3').value = s.img.height ? s.img.height : '';
			$(editorid + '_image_status').innerHTML = '';
		} else {
			$(editorid + '_image_submit').disabled = true;
			$(editorid + '_image_status').innerHTML = ' 验证图片中...';
			setTimeout(function () { s.loadCheck(); }, 100);
		}
	};
	s.loadCheck();
}

function addpolloption() {
	if(curoptions < maxoptions) {
		$('polloption_new').outerHTML = '<p>' + $('polloption_hidden').innerHTML + '</p>' + $('polloption_new').outerHTML;
		curoptions++;
	}
}

function delpolloption(obj) {
	obj.parentNode.parentNode.removeChild(obj.parentNode);
	curoptions--;
}



function showsmiles1(index, typename, pageindex, seditorKey)

{

	$("s_" + index).className = "current";

	var cIndex = 1;

	for (i in smilies_HASH) {

		if (cIndex != index) {

			$("s_" + cIndex).className = "";

		}

		$("s_" + cIndex).style.display = "";

		cIndex ++;

	}



	var pagesize = (typeof smiliesCount) == 'undefined' ? 12 : smiliesCount;

	var url = (typeof forumurl) == 'undefined' ? '' : forumurl;

	var s = smilies_HASH[typename];

	var pagecount = Math.ceil(s.length/pagesize);

	var inseditor = typeof seditorKey != 'undefined';



	if (isUndefined(pageindex)) {

		pageindex = 1;

	}

	if (pageindex > pagecount) {

		pageindex = pagecount;

	}



	var maxIndex = pageindex*pagesize;

	if (maxIndex > s.length) {

		maxIndex = s.length;

	}

	maxIndex = maxIndex - 1;



	var minIndex = (pageindex-1)*pagesize;



	var html = '<table id="' + index + '_table" cellpadding="0" cellspacing="0" style="clear: both"><tr>';



	var ci = 1;

	for (var id = minIndex; id <= maxIndex; id++) {

		var clickevt = 'insertSmiley(\'' + addslashes(s[id]['code']) + '\');';

		if (inseditor) {

			clickevt = 'seditor_insertunit(\'' + seditorKey + '\', \'' + s[id]['code'] + '\');';

		}



		html += '<td valign="middle"><img style="cursor: pointer;" src="' + url + 'editor/images/smilies/' + s[id]['url'] + '" id="smilie_' + s[id]['code'] + '" alt="' + s[id]['code'] + '" onclick="' + clickevt + '" onmouseover="smilies_preview(\'s\', this, 40)" onmouseout="smilies_preview(\'s\')" title="" border="0" height="20" width="20" /></td>';

		if (ci%colCount == 0) {

			html += '</tr><tr>'

		}

		ci ++;

	}



	html += '<td colspan="' + (colCount - ((ci-1) % colCount)) + '"></td>';

	html += '</tr>';

	html += '</table>';

	$("showsmilie").innerHTML = html;



	if (pagecount > 1) {

		html = '<div class="p_bar">';

		for (var i = 1; i <= pagecount; i++) {

			if (i == pageindex) {

				html += "<a class=\"p_curpage\">" + i + "</a>";

			}

			else {

				html += "<a class=\"p_num\" href='#smiliyanchor' onclick=\"showsmiles1(" + index + ", '" + typename + "', " + i + ")\">" + i + "</a>"

			}

		}

		html += '</div>'

		$("showsmilie_pagenum").innerHTML = html;

	}

	else {

		$("showsmilie_pagenum").innerHTML = "";

	}

}

function switchAdvanceMode(url,form) {

	//var obj = $('postform') && (($('fwin_newthread') && $('fwin_newthread').style.display == '') || ($('fwin_reply') && $('fwin_reply').style.display == '')) ? $('postform') : $('quickpostform');

	if($(form).message.value != '') {

		saveData(undefined,form);

		url += (url.indexOf('?') != -1 ? '&' : '?') + 'cedit=yes';
	}
	location.href = url;
	return false;

}

function insertAllAttachTag() {
    var attachListObj = $('e_attachlist').getElementsByTagName("tbody");
    for (var i in attachListObj) {
        if (typeof attachListObj[i] == "object") {
            var attach = attachListObj[i];
            var ids = attach.id.split('_');
            if (ids[0] == 'attach') {
                if ($('attachname' + ids[1])) {
                    if (parseInt($('attachname' + ids[1]).getAttribute('isimage'))) {
                        insertAttachimgTag(ids[1]);
                    } else {
                        insertAttachTag(ids[1]);
                    }
                }
            }
        }
    }
    doane();
}