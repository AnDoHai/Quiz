/**
 * @license Copyright (c) 2003-2021, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Toolbar configuration generated automatically by the editor based on config.toolbarGroups.
	
	config.toolbarGroups = [
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align'] },
		{ name: 'styles' },
		{ name: 'colors' },
		{ name: 'insert' },
		{ name: 'others' }
	];
	config.extraPlugins = 'pastebase64';
	config.height = 240;
	// Remove some buttons, provided by the standard plugins, which we don't
	// need to have in the Standard(s) toolbar.
	config.removeButtons = 'Subscript,Superscript';
};
