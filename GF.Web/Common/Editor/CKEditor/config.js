/*
Copyright (c) 2003-2010, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'zh-cn';
	config.uiColor = '#EDEDED';
	config.toolbar = 'MyToolbar';
	config.toolbar_MyToolbar =
	[
		['PageBreak'],
		['Cut','Copy','Paste','PasteText','RemoveFormat'],
		['Undo','Redo'],
		['Bold','Italic','Underline','Strike'],
		['TextColor','BGColor'],
		['Image','Flash','mp3','syntax','Table','SpecialChar'],
		['Maximize'],
		'/',
		['NumberedList','BulletedList','-','Outdent','Indent','Blockquote'],
		['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
		['Link','Unlink','-','Find','Replace'],
		['Font','FontSize'],
		['Source']
	];
	config.toolbarCanCollapse = false;
	// config.extraPlugins = 'media';
};
