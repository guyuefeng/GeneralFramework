CKEDITOR.dialog.add('syntax',function(editor){
	var escape=function(text)
	{
		return text.replace(/&/g, '&amp').replace(/\"/g, '&quot;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
	};
	return{
		title:'Code',
		resizable:CKEDITOR.DIALOG_RESIZE_BOTH,
		minWidth:600,
		minHeight:300,
		contents:[{
			id:'cb',
			name:'cb',
			label:'cb',
			title:'cb',
			elements:[{
				type:'select',
				label:'Language',
				id:'lang',
				required:true,
				'default':'csharp',
				items:[['XHTML/XML','html'],['JavaScript','javascript'],['CSS','css'],['PHP','php'],['C#','csharp'],['C++','cpp'],['Java','java'],['Python','python'],['Ruby','ruby'],['Visual Basic','vb'],['Delphi','delphi'],['SQL','sql'],['Text','plain']]
				},
				{
					type:'textarea',
					style:'width:600px;height:300px',
					label:'Code',
					id:'code',
					rows:15
				}]
		}],
		onOk:function()
		{
			code=this.getValueOf('cb','code');
			lang=this.getValueOf('cb','lang');
			html=''+escape(code)+'';
			editor.insertHtml("<pre name=\"code\" class=\""+lang+"\">"+html+"</pre>");
		},
		onLoad:function()
		{
		}
	};
});