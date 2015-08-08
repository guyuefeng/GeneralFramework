CKEDITOR.plugins.add( 'mp3',
{
	requires :['dialog'],
	init : function( editor )
	{
		var command = editor.addCommand( 'mp3', new CKEDITOR.dialogCommand('mp3'));
		//command.modes = { wysiwyg:1, source:1 };
		command.canUndo = false;

		editor.ui.addButton('mp3',
			{
				label:'MP3',
				command:'mp3',
				icon:this.path+'images/mp3.gif',
			});

		CKEDITOR.dialog.add('mp3',this.path+'dialogs/mp3.js' );
	}
});