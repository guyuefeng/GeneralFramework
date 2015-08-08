CKEDITOR.dialog.add('mp3',function(editor){
		return{
			title:'MP3',
			resizable:CKEDITOR.DIALOG_RESIZE_BOTH,
			minWidth:400,
			minHeight:100,
			contents:[{
				id:'mp3Path',
				name:'',
				label:'',
				title:'',
				elements:[{
					type:'text',
					label:'MP3 URI',
					id:'path',
					required:true
					}]
				}],
				onOk:function(){
					//alert(CKEDITOR.basePath.replace('/CKEditor',''));
					//return false;
					pathUrl=this.getValueOf('mp3Path','path');
					//html='<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"><param name="quality" value="high" /><param name="movie" value="/Common/Editor/player.swf?soundFile='+pathUrl+'&amp;t=swf" /><embed pluginspage="http://www.macromedia.com/go/getflashplayer" quality="high" src="/Common/Editor/player.swf?soundFile='+pathUrl+'&amp;t=swf" type="application/x-shockwave-flash"></embed></object>';
					html='<embed type="application/x-shockwave-flash" classid="clsid:d27cdb6e-ae6d-11cf-96b8-4445535400000" src="'+CKEDITOR.basePath.replace('/CKEditor','')+'player.swf?soundFile='+pathUrl+'&amp;t=swf" wmode="opaque" quality="high" menu="false" play="true" loop="true" allowfullscreen="true" height="26" width="300" />';
					editor.insertHtml(html);
				},
				onLoad:function(){
				}
			};
});