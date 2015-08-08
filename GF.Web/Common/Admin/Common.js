$(document).ready(function()
{
	//点击后取消焦点
	$("a, button").click(function()
	{
		this.blur();
	});
	//菜单部分
	var menuRel = $.cookie("thisMenu");
	if (menuRel < 1) { menuRel = 1; }
	$(".left .menuCap").removeClass("this");
	$(".left .item .content").slideUp(100);
	$(".left .menuCap[rel=" + menuRel + "]").addClass("this");
	$(".left #menu_" + menuRel + " .content").slideDown(100);
	$(".menuCap").click(function()
	{
		var _this = this;
		$.cookie("thisMenu", _this.rel);
		$(".left .item .content").slideUp(100);
		$(".left #menu_" + _this.rel + " .content").slideDown(100, function()
		{
			$(".menuCap").removeClass("this");
			$(_this).addClass("this");
		});
	});

	//附件插入部分
	var addFilesCount = 0;
	var InsertToEditor = function(editorId, html)
	{
		var oEditor = CKEDITOR.instances[editorId];
		// Get the editor contents
		if (oEditor.mode == 'wysiwyg')
		{
			// Insert the desired HTML.
			oEditor.insertHtml(html);
		}
	}
	$("#filesMainInsertA").click(function()
	{
		InsertToEditor("content", "[LocalUpload_0]");
	});
	$("#addFilesA").click(function()
	{
		addFilesCount++;
		$($("#filesMainBox").html()).appendTo("#filesAttBox");
		$("<a rel=\"" + addFilesCount + "\">插入</a><br/>").appendTo("#filesAttBox").click(function()
		{
			InsertToEditor("content", "[LocalUpload_" + this.rel + "]");
		});
	});

	//筛选标签列表并填充
	$("#getInputTags").click(function()
	{
		var thisObj = $(this);
		thisObj.hide();
		$.ajax({
			url : "Service.aspx?act=matchTags",
			type : "POST",
			dataType : "xml",
			success : function(data)
			{
				if ($(data).find("result").text())
				{
					$("#getInputTagsDsp").html($(data).find("result").text());
					$("#getInputTagsDsp a").click(function()
					{
						var tagTxt = this.innerHTML;
						var tagsObj = $("#" + this.rel);
						if (tagsObj.val()) { tagsObj.val(tagsObj.val() + "," + tagTxt); }
						else { tagsObj.val(tagTxt); }
					});
				}
				else { thisObj.show(); }
			},
			error : function (e) { thisObj.show(); }
		});
	});
	
	/*表情显示部分*/
	$(".showEmotsBox").click(function()
	{
		$(".emots").toggle();
	});
	$(".emotLink").click(function()
	{
		InsertToEditor(this.rel, this.innerHTML);
	});

	//自动保存
	if ($("#autoSaveCountdown").length > 0)
	{
		var autoSaveTimer = 30;
		var autoSaveCD = autoSaveTimer;
		var autoSaveBtnStop = false;
		var autoSaveExe = function()
		{
			$("#autoSaveCountdown").html(autoSaveCD);
			if (autoSaveCD > 0) { autoSaveCD--; }
			else
			{
				autoSaveCD = autoSaveTimer;
				var oEditor = CKEDITOR.instances.content;
				$("#autoSaveIs").html("正在保存…").fadeIn(500);
				$.ajax({
					url : "Service.aspx?act=autoSave",
					type : "POST",
					data : { content : oEditor.getData() },
					dataType : "xml",
					success : function(data)
					{
						$("#autoSaveIs").html($(data).find("msg").text()).fadeOut(3000);
					},
					error : function (e) { $("#autoSaveIs").html("保存出错！").fadeOut(3000); }
				});
			}
		}
		var autoSaveSI = setInterval(autoSaveExe, 1000);
		$("#autoSaveButton").html("暂停").click(function()
		{
			if (autoSaveBtnStop)
			{
				autoSaveSI = setInterval(autoSaveExe, 1000);
				$(this).html("暂停");
				autoSaveBtnStop = false;
			}
			else
			{
				clearInterval(autoSaveSI);
				$(this).html("开始");
				autoSaveBtnStop = true;
			}
		});
	}
	
	$("#reInsertContent").click(function()
	{
		$.ajax({
			url : "Service.aspx?act=rePostContent",
			type : "POST",
			dataType : "xml",
			success : function(data)
			{
				var oEditor = CKEDITOR.instances.content;
				oEditor.setData($(data).find("result").text());
			},
			error : function (e) { alert("获取出错！"); }
		});
		$(this).fadeOut(1000);
	});
	
	$("#advancedLink").click(function()
	{
		$("table.hidden").toggle();
		//$(".advancedBox").toggle();
	});

	/*更新时间*/
	$("#updateTime").click(function()
	{
		$.ajax({
			url : "Service.aspx?act=getTime",
			type : "POST",
			dataType : "xml",
			success : function(data)
			{
				$("#timeInput").val($(data).find("result").text());
			},
			error : function (e) { alert("取值出错！"); }
		});
	});

	/*在线更新*/
	$("#upgradeBegin").click(function()
	{
		$(this).attr("disabled", "disabled");
		$("#preDemo").width("20%");
		$("#upgradeState").html("正在下载更新文件，请稍后…");
		var verNum = $("#upgradeVer").val();
		var verToken = $("#upgradeToken").val();
		$.ajax({
			url : "Upgrade.aspx?act=down",
			type : "POST",
			data : { token : verToken, ver : verNum },
			success : function(data)
			{
				if (data == "1")
				{
					$("#preDemo").width("50%");
					$("#upgradeState").html("下载成功，正在安装…");
					$.ajax({
						url : "Upgrade.aspx?act=inst",
						type : "POST",
						data : { token : verToken, ver : verNum },
						success : function(data)
						{
							if (data == "1")
							{
								$("#preDemo").width("100%");
								$("#upgradeState").html("安装成功，正在重启…");
								alert("重启应用程序…");
								top.location.reload();
							}
							else
							{
								$("#upgradeState").html(data);
							}
						},
						error : function (e) { alert("程序出错，未安装成功，也许您只能手动更新了！"); }
					});
				}
				else
				{
					$("#upgradeState").html(data);
				}
			},
			error : function (e) { alert("程序出错，未下载成功！"); }
		});
	});
});