/**
 * Copyright (c) 2014-2021, CKSource - Frederico Knabben. All rights reserved.
 * Licensed under the terms of the MIT License (see LICENSE.md).
 *
 * Basic sample plugin inserting abbreviation elements into the CKEditor editing area.
 *
 * Created out of the CKEditor Plugin SDK:
 * https://ckeditor.com/docs/ckeditor4/latest/guide/plugin_sdk_sample_1.html
 */

// Register the plugin within the editor.
CKEDITOR.plugins.add( 'abbr', {

	// Register the icons.
	icons: 'abbr',

	// The plugin initialization logic goes inside this method.
	init: function( editor ) {
		// Define an editor command that opens our dialog window.
		editor.addCommand( 'abbr', new CKEDITOR.dialogCommand( 'abbrDialog' ) );

		// Create a toolbar button that executes the above command.
		editor.ui.addButton( 'Abbr', {

			// The text part of the button (if available) and the tooltip.
			label: 'Insert Abbreviation',

			// The command to execute on click.
			command: 'abbr',

			// The button placement in the toolbar (toolbar group name).
			toolbar: 'insert'
		});

		// Register our dialog file -- this.path is the plugin folder path.
		CKEDITOR.dialog.add( 'abbrDialog', function (editor) {
            return {
                title: 'Upload ảnh',
                minWidth: 400,
                minHeight: 200,
                contents:
				[
					{
					    id: 'tab1',
					    label: 'Basic Settings',
                        elements:
						[
                            {
                                type: 'html',
                                html: "<div><form id='form1' enctype='multipart/form-data' method='post' action='Upload.aspx'><div class='row'><label for='fileToUpload'>Select a File to Upload</label><br /><input type='file' name='fileToUpload' id='fileToUpload' onchange='fileSelected();'/></div><div id='fileName'></div><div id='fileSize'></div><div id='fileType'></div><div class='row'></div><div id='progressNumber'></div></form></div>"
                            }
						]
					}
				],
                onOk: function () {
                    var ketqua = JSON.parse( uploadFile() );
                    editor.insertHtml("<img src='"+ ketqua.url+ "' />" );
                }
            };
        });
	}
});

/**************************************Upload file **********************************/
function fileSelected() {
var file = document.getElementById('fileToUpload').files[0];
if (file) {
    var fileSize = 0;
    if (file.size > 1024 * 1024)
    fileSize = (Math.round(file.size * 100 / (1024 * 1024)) / 100).toString() + 'MB';
    else
    fileSize = (Math.round(file.size * 100 / 1024) / 100).toString() + 'KB';

    document.getElementById('fileName').innerHTML = 'Name: ' + file.name;
    document.getElementById('fileSize').innerHTML = 'Size: ' + fileSize;
    document.getElementById('fileType').innerHTML = 'Type: ' + file.type;
}
}

function uploadFile() {
    var fd = new FormData();
    fd.append("fileToUpload", document.getElementById('fileToUpload').files[0]);
    var xhr = new XMLHttpRequest();
    xhr.upload.addEventListener("progress", uploadProgress, false);
    xhr.addEventListener("load", uploadComplete, false);
    xhr.addEventListener("error", uploadFailed, false);
    xhr.addEventListener("abort", uploadCanceled, false);
    xhr.open("POST", "/Upload/UploadUsingCK/" , false);
    xhr.send(fd);
    return(xhr.responseText);
}

function uploadProgress(evt) {
    if (evt.lengthComputable) {
        var percentComplete = Math.round(evt.loaded * 100 / evt.total);
        document.getElementById('progressNumber').innerHTML = percentComplete.toString() + '%';
    }
    else {
        document.getElementById('progressNumber').innerHTML = 'unable to compute';
    }
}

function uploadComplete(evt) {
    return evt.target.responseText;
}

function uploadFailed(evt) {
    alert("There was an error attempting to upload the file.");
}
      
function uploadCanceled(evt) {
    alert("The upload has been canceled by the user or the browser dropped the connection.");
}
