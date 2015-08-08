CKEDITOR.plugins.add( 'syntax',
{
	requires :['dialog'],
	init : function( editor )
	{
		var command = editor.addCommand( 'syntax', new CKEDITOR.dialogCommand('syntax'));
		command.modes = { wysiwyg:1, source:1 };
		command.canUndo = false;

		editor.ui.addButton('syntax',
			{
				label:'Code',
				command:'syntax',
				icon:this.path+'images/code.gif',
			});

		CKEDITOR.dialog.add('syntax',this.path+'dialogs/syntax.js' );
	}
});
