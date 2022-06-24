var app = angular.module('myApp', []);  //创建模块
ExcelList = null; //excel表的数据
dstime = null;  //记录定时器
yy = 0; //记录进度条
var DataIndexList = []; //需要标红的数据序号集合，已作废
var NullData = false; //是否为空数据
app.controller('GetK3POInformationController', function ($scope) {
    $('#excel-file').change(function (e) {
        var files = e.target.files;
        var fileReader = new FileReader();
        //得到上传文件的值
        var fileName = document.getElementById("jobData").value;
        //返回String对象中子字符串最后出现的位置.
        var seat = fileName.lastIndexOf(".") + 1;
        //返回位于String对象中指定位置的子字符串并转换为小写.
        var extension = fileName.substring(seat).toLowerCase();
        if (extension == "") {
            return false;
        }
        //禁用开始采集按钮
        $('#two').css("pointer-events", "none").attr('disabled', "true");
        jdt(0);
        yy = 0;
        $("#myModal").modal({ backdrop: 'static', keyboard: false });
        dstime = setInterval(function () {
            if (yy < 50) {
                yy++;
                jdt(yy); //进度条赋值
            }
        }, 300)
        if ("csv" != extension) {
            swal('导入失败，只能上传后缀名为csv的文件，请下载页面右上角的装箱表模板重新导入！', '', 'error');
            //关闭遮罩层，清空
            $scope.globalmodal(false);
            return false;
        }
        fileReader.onload = function (ev) {
            try {
                var data = ev.target.result,
                    workbook = XLSX.read(data, {
                        type: 'binary'
                    }), // 以二进制流方式读取得到整份excel表格对象
                    persons = []; // 存储获取到的数据
                if (data.indexOf("CurrencyID") == -1 && data.indexOf("InvUPrice") == -1) {
                    swal('导入失败，请使用最新的装箱表格式重新导入，请下载页面右上角的装箱表模板重新导入！', '', 'error');
                    //关闭遮罩层，清空
                    $scope.globalmodal(false);
                    return false;
                }
            }
            catch (e) {
                //格式不正确
                console.log('文件类型不正确');
                swal('导入失败，错误的装箱格式，请核对后重新上传，请下载页面右上角的装箱表模板重新导入！', 'error');
                //关闭遮罩层，清空
                $scope.globalmodal(false);
                return false;
            }

            // 表格的表格范围，可用于判断表头是否数量是否正确
            var fromTo = '';
            // 遍历每张表读取
            for (var sheet in workbook.Sheets) {
                if (workbook.Sheets.hasOwnProperty(sheet)) {
                    fromTo = workbook.Sheets[sheet]['!ref'];
                    /*console.log(fromTo);*/
                    persons = persons.concat(XLSX.utils.sheet_to_json(workbook.Sheets[sheet]));
                    break; // 如果只取第一张表，就取消注释这行
                }
            }
            ExcelList = persons;
            /*console.log(persons);*/
            //清空表格
            //$(".tablebody").html("");
            var q = 0;
            var tempLength = persons.length;
            var j = 0;
            var loop = function () {
                if (j >= tempLength) {
                    //退出循环
                    //$scope.Startbtn(0); //批量插入再查询出来
                    $scope.NewStartbtn(0);
                    return;
                }
                if (j == 0) {
                    //console.log(new Array(persons[0])[0]["GIP-PO"])
                    //2022-6-14新增两个字段CurrencyID和InvUPrice，可为空
                    if (persons[0].CurrencyID == undefined && persons[0].InvUPrice != undefined) {
                        persons[0] = new Object({ "Serial-No": new Array(persons[0])[0]["Serial-No"], "GIP-PO": new Array(persons[0])[0]["GIP-PO"], "Part-No": new Array(persons[0])[0]["Part-No"], "Qty": new Array(persons[0])[0]["Qty"], "Unit": new Array(persons[0])[0]["Unit"], "CurrencyID": " ", "InvUPrice": new Array(persons[0])[0]["InvUPrice"] });
                    }
                    if (persons[0].CurrencyID != undefined && persons[0].InvUPrice == undefined) {
                        persons[0] = new Object({ "Serial-No": new Array(persons[0])[0]["Serial-No"], "GIP-PO": new Array(persons[0])[0]["GIP-PO"], "Part-No": new Array(persons[0])[0]["Part-No"], "Qty": new Array(persons[0])[0]["Qty"], "Unit": new Array(persons[0])[0]["Unit"], "CurrencyID": new Array(persons[0])[0]["CurrencyID"], "InvUPrice": " " });
                    }
                }
                var arr = persons[j];
                //console.log(persons[0]);
                var flag = true;
                for (var i in arr) {
                    i = i.toLowerCase();
                    if (j == 0) {
                        if (q == 0 && i.toString() != "serial-no") {  //标题1

                            flag = false;
                        } else if (q == 1 && i.toString() != "gip-po") {

                            flag = false;
                        } else if (q == 2 && i.toString() != "part-no") {

                            flag = false;
                        } else if (q == 3 && i.toString() != "qty") {

                            flag = false;
                        }
                        else if (q == 4 && i.toString() != "unit") {

                            flag = false;
                        }
                        //2022-6-14新增两个字段CurrencyID和InvUPrice*/
                        else if (q == 5 && i.toString() != "currencyid") {

                            flag = false;
                        }
                        else if (q == 6 && i.toString() != "invuprice") {

                            flag = false;
                        }
                        if (!flag) {
                            //关闭遮罩层，清空
                            $scope.globalmodal(false);
                            swal('导入失败，错误的装箱格式，请核对后重新上传！，错误出在<' + i + '>标题上', '', 'error');
                            ExcelList = null;
                            return false;
                        }
                    }
                    q++;
                }
                j++;
                //下一步循环  
                this.window.setTimeout(loop, 0); //递归
            }
            this.loopId = window.setTimeout(loop, 0);
            {
                /*因为for循环的机制问题，改用递归
                for (let j = 0; j < persons.length; j++) {
                    (function (j) {
                        var processCount = (j+1) * 100 / persons.length;
                        document.haorooms.haoroomsinput0.value = processCount;
                        //$("#tt0").html(processCount + "%");
                        document.getElementById("tt0").innerHTML = processCount + "%";
                        document.getElementById("tt0").style.width = processCount + "%";
                        //$("#tt0").css("width", processCount + "%");
                        console.log(processCount);
                    })(j);
             
                    var arr = persons[j];
                    //if (j == 0) {
                    //    $(".tablehead").append("<tr class='exceltitle'></tr>");
                    //}
                    $(".tablebody").append("<tr class='excelcontent'></tr>");
                    rowSortNumber++;
                    $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' >" + rowSortNumber + "</td>");
                    $(".tablebody").eq(1).children("tr").eq(j).append("<td class='red' >" + rowSortNumber + "</td>");
             
                    for (var i in arr) {
                        //alert(i+"---"+arr[i]);
                        if (j == 0 && q == 0) {
                            //$(".exceltitle").append("<th>" + i + "</th>");
                            var aa = i.toString();
                            if (aa != "GIP-PO") {
                                $(".tablebody").html("");
                                alert("错误的装箱表文件，请重新上传！");
                                ExcelList = null;
                                return false;
             
                            }
                            q++;
                        }
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:right'>" + arr[i] + "</td>");
                        $(".tablebody").eq(1).children("tr").eq(j).append("<td class='red'>" + arr[i] + "</td>");
                    }
                    for (var i = 0; i < 9; i++) {   //生成紫色空白行
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='violet'><a></a></td>");
                        $(".tablebody").eq(1).children("tr").eq(j).append("<td class='violet'><a></a></td>");
                    }
                    for (var i = 0; i < 4; i++) {  //生成灰色空白行
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='grey'></td>");
                        $(".tablebody").eq(1).children("tr").eq(j).append("<td class='grey'></td>");
                    }
                    //sleep(300);
                }
                */
                //alert("导入成功！");
                // $scope.globalmodal(false);
            }
        };
        // 以二进制方式打开文件
        fileReader.readAsBinaryString(files[0]);
        //用完之后清空文件值
        $("#jobData").val("");
    });

    //开始采集按钮
    $scope.Startbtn = function (urlcanshu) {
        $("#myModal").modal({ backdrop: 'static', keyboard: false });
        jdt(0);
        yy = 0;
        dstime = setInterval(function () {
            if (yy < 50) {
                yy++;
                jdt(yy); //进度条赋值
            }
        }, 300)
        if (urlcanshu == 0) {  //调插入方法
            urlcanshu = "/GetK3POInformation/AddLoadingListAddPOdata_Temp/AddFunction";
            data = $.param({ 'ArrayList': JSON.stringify(ExcelList) }) + '&';   //将Array转换为JSON字符串传入后台
        }
        else {
            urlcanshu = "/GetK3POInformation/GetK3POInformation/SelectK3PO_Num";
            data = null;
        }
        $.ajax({
            type: "post",
            async: true,
            dataType: 'json',
            url: urlcanshu,
            data: data,
            success: function (data) {
                $(".tablebody").html("");
                //调用新增方法
                if (urlcanshu == "/GetK3POInformation/AddLoadingListAddPOdata_Temp/AddFunction") {
                    //if (data == 0) {
                    //    alert("没有完成导入工作，数据格式错误，请检查原始数据！");//新增出现问题
                    //}
                    $scope.List(1, data);   //导入
                }
                else {
                    if (data == "") {   //修改出现问题
                        swal('发生错误，请联系电脑部！内部成员请查看日志文件！', '', 'error');
                    }
                    $scope.List(2, data.Success, data.Msg);  //采集
                }


            },
            error: function () {
                swal('发生错误，请联系电脑部！内部成员请查看日志文件！', '', 'error');
            }
        })
    }

    //优化版：开始采集按钮，抓出数据(0-导入)
    $scope.NewStartbtn = function (urlcanshu) {
        if (urlcanshu != 0) {
            jdt(0);
            yy = 0;
            $("#myModal").modal({ backdrop: 'static', keyboard: false });
            dstime = setInterval(function () {
                if (yy < 50) {
                    yy++;
                    jdt(yy); //进度条赋值
                }
            }, 300)
        }
        if (urlcanshu == 0) {  //调插入方法
            urlcanshu = "/GetK3POInformation/AddLoadingListAddPOdata_Temp/AddFunction";
            data = $.param({ 'ArrayList': JSON.stringify(ExcelList) }) + '&';   //将Array转换为JSON字符串传入后台
        }
        else {
            /*urlcanshu = "/GetK3POInformation/GetK3POInformation/SelectK3PO_Num";*/
            urlcanshu = "/GetK3POInformation/AddLoadingListAddPOdata_Temp/NewUpdFunction";
            data = null;
        }
        $.ajax({
            type: "post",
            async: true,
            dataType: 'json',
            url: urlcanshu,
            data: data,
            success: function (data) {
                $(".tablebody").html("");
                //调用新增方法
                if (urlcanshu == "/GetK3POInformation/AddLoadingListAddPOdata_Temp/AddFunction") {
                    //if (data == 0) {
                    //    alert("没有完成导入工作，数据格式错误，请检查原始数据！");//新增出现问题
                    //}
                    $scope.List(1, data);   //导入
                }
                else {
                    if (data == "") {   //修改出现问题
                        //关闭遮罩层，清空
                        $scope.globalmodal(false);
                        swal('发生错误，请联系电脑部！内部成员请查看日志文件！', '', 'error');
                    }
                    $scope.List(2, data.Success, data);  //采集
                }


            },
            error: function () {
                //关闭遮罩层，清空
                $scope.globalmodal(false);
                swal('发生错误，请联系电脑部！内部成员请查看日志文件！', '', 'error');
            }
        })
    }

    //关闭弹出框，清空值和定时器
    $scope.globalmodal = function (action) {
        /*全局遮罩层*/
        var mod = $("#myModal");//全局遮罩层
        if (action == true) {
            //打开遮罩层
            /* mod.modal();*/
        }
        else {
            /*关闭遮罩层*/
            jdt(0);
            yy = 0;
            TrCount = $(".tablebody tr").length; //页面上的数据
            //数据不为空 ，打开开始采集按钮
            if (TrCount > 0 && NullData == false) {
                $('#two').removeAttr("disabled").css("pointer-events", "auto");
            }
            window.clearInterval(dstime);//清空定时器
            mod.modal('hide');
            NullData = false; //标志位初始化
        }
        /*遮罩层样式及位置*/
        //mod.height(element.height() + 10);//遮罩层高度
        //mod.width(element.width());//设置遮罩层宽度
        //mod.offset(element.offset());//根据遮罩对象来进行定位
    }

    //0-进入页面加载，1-导入数据，2-采集数据
    $scope.List = function (canshu, SuccessMsg, DataList) {
        var tempLength;  //存放数据总条数
        if (canshu == 0) {    //首页加载进度条
            $("#XMModal").modal({ backdrop: 'static', keyboard: false }); //打开熊猫加载图片
        }

        //开始采集
        if (canshu == 2) {
            window.clearInterval(dstime);//清空定时器
            var tempLength;
            if (DataList == null || DataList == "") {
                tempLength = 0
            } else {
                tempLength = DataList.length;
            }
            $scope.FillData(canshu, tempLength, DataList, SuccessMsg);
        }
        else {
            //加载数据
            $.ajax({
                type: "post",
                async: true,
                dataType: 'json',
                url: "/GetK3POInformation/GetK3POInformation/NewIndexData",
                success: function (result) {
                    //将需要标红的数据序号集合添加进数组(作废)
                    //DataIndexList.push(result.DataIndexList);
                    //console.log(DataIndexList);
                    window.clearInterval(dstime);//清空定时器
                    if (result == null || result == "") {
                        tempLength = 0
                    } else {
                        tempLength = result.length;
                    }
                    $scope.FillData(canshu, tempLength, result, SuccessMsg);
                },
                error: function () {
                    //关闭遮罩层，清空
                    $scope.globalmodal(false);
                    swal('发生错误，请联系电脑部！内部成员请查看日志文件！', '', 'error');
                }
            })
        }


        /*DaoChuList();*/
        //用完之后清空数组
        DataIndexList = [];
    }

    //将数据填充在表格里:canshu-0-进入页面加载，1-导入数据，2-采集数据
    $scope.FillData = function (canshu, tempLength, Data, SuccessMsg) {
        var j = 0;
        var index = 0;
        if (tempLength != 0) {
            var loop = function () {
                if (j >= tempLength) {
                    //退出循环
                    if (canshu == 0) {
                        //加载完成后  关闭熊猫遮罩层
                        $("#XMModal").modal('hide');
                    }
                    setTimeout(function () {
                        $scope.globalmodal(false);
                        if (canshu == 1) {  //导入数据
                            if (SuccessMsg == 0) {  //新增失败
                                swal('没有完成导入工作，数据格式错误，请检查原始数据！', '', 'error');
                            }
                            else {
                                swal('导入成功!共导入' + tempLength + '条数据!', '', 'success') //提示框
                                $('#two').removeAttr("disabled").css("pointer-events", "auto"); //打开开始采集按钮
                            }

                        }
                        if (canshu == 2 && SuccessMsg == 0) {  //采集数据没成功，成功则不用提示
                            swal('发生错误，请联系电脑部！内部成员请查看日志文件！', '', 'error');
                        }
                        if (canshu != 0) {
                            jdt(0);
                            yy = 0;
                        }

                    }, 1000);
                    return;
                }
                $(".tablebody").append("<tr class='excelcontent'></tr>");
                $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:cneter'>" + Data[j].LPSerial + "</td>");
                $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + Data[j].PONum + "</td>");
                $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + Data[j].Fnumber + "</td>");
                $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:right'>" + Data[j].LoadQty + "</td>");
                $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + Data[j].LoadUnit + "</td>");
                //2022-6-14新增两个字段LoadQty和LoadUnit
                $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:right'>" + Data[j].LoadCurr + "</td>");
                $(".tablebody").eq(0).children("tr").eq(j).append("<td class='red' style='text-align:center'>" + Data[j].LoadUPrice + "</td>");
                if (canshu == 1) {
                    for (var i = 0; i <= 8; i++) {  //导入的时候不需要三行导出的数据
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='violet'></td>");
                    }
                }
                else {
                    for (var i = 0; i <= 12; i++) {
                        $(".tablebody").eq(0).children("tr").eq(j).append("<td class='violet'></td>");
                    }
                }
                for (var i = 0; i <= 3; i++) {
                    $(".tablebody").eq(0).children("tr").eq(j).append("<td class='grey'></td>");
                }
                //采集数据/进入页面数据加载
                if (canshu != 1) {
                    //采集数据从7开始
                    //第7行显示Ledger（账套）
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(7).html(Data[j].Ledger);

                    //供货商（Supplier）数据超过三行显示省略号+鼠标悬浮（a标签）出现全部内容
                    //导出显示全部内容 （第8行导出显示Supplier）
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(8).html(Data[j].Supplier).addClass("DaoChuClass");

                    //不导出，第9行显示Supplier（供货商）
                    if (Data[j].Supplier != null && Data[j].Supplier.length >= 50 && Data[j].Supplier != '') {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(9).prop("title", Data[j].Supplier);
                        Data[j].Supplier = Data[j].Supplier.substring(0, 50) + "...";
                    }
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(9).html(Data[j].Supplier).css({ "text-align": "left" }).addClass("noExl");
                    //给第9行加样式
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(9).css({ "text-align": "left" });



                    //物料名称/规格/制造商（Material detail）数据超过三行显示省略号+鼠标悬浮（a标签）出现全部内容
                    //导出显示全部内容  （第10行导出显示Material detail）
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(10).html(Data[j].Material).addClass("DaoChuClass");

                    //不导出，第11行显示Material（物料名称/规格/制造商）
                    if (Data[j].Material != null && Data[j].Material.length >= 60 && Data[j].Material != '') {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(11).prop("title", Data[j].Material);
                        Data[j].Material = Data[j].Material.substring(0, 60) + "...";
                    }
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(11).html(Data[j].Material).css({ "text-align": "left" }).addClass("noExl");
                    //给第11行加样式
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(11).css({ "text-align": "left" });



                    //第12行显示POQty（数量）
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(12).html(Data[j].POQty).css({ "text-align": "right" });
                    //第13行显示POUnit（单位）
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(Data[j].POUnit);
                    //第14行显示POCurr（货币）
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(14).html(Data[j].POCurr);

                    //第15行（UnitPrice）特殊处理
                    //装箱表的货币或者价格不相同时，导出显示[！PO$]（第15行导出显示UnitPrice）
                    if (Data[j].ISPOIcon == 1) {
                        var span = "<span>[！PO$]</span>    ";
                        var UnitPrice = "<span style='color:black'>" + Data[j].UnitPrice + "</span>";
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(15).html(span + '<br style="mso-data-placement:same-cell;"/>' + UnitPrice).css({ "text-align": "right" }).addClass("DaoChuClass").addClass("fontred");//po单价
                    }
                    else {
                        //导出没标志
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(15).html(Data[j].UnitPrice).css({ "text-align": "right" }).addClass("DaoChuClass");//po单价
                    }
                    //装箱表的货币或者价格不相同时，显示图标(第16行)
                    if (Data[j].ISPOIcon == 1) {
                        //加一个小图标
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(16).html('<img src="./Scripts/Images/ISPOIcon.gif"  style="float:left"/>' + Data[j].UnitPrice).css({ "text-align": "right" }).addClass("noExl");//po单价
                    }
                    else {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(16).html(Data[j].UnitPrice).css({ "text-align": "right" }).addClass("noExl");//po单价
                    }
                    //第17行交货日期（Delivery）格式处理
                    if (Data[j].NeedDate != null) {
                        Data[j].NeedDate = new Date(Data[j].NeedDate).Format("yyyy/MM/dd");  //日期格式化;
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(17).html(Data[j].NeedDate);
                    }
                    else {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(17).html("");
                    }
                    if (Data[j].NeedDate == '1900/01/01') {   //进行二次判断
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(17).html("");
                    }

                    //备注（Remark）数据超过三行显示省略号+鼠标悬浮（a标签）出现全部内容
                    //导出显示全部内容（第18行导出显示Remark）
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(18).html(Data[j].Remarks).addClass("DaoChuClass");

                    //不导出，第19行显示Remark（备注）
                    if (Data[j].Remarks != null && Data[j].Remarks.length >= 40 && Data[j].Remarks != '') {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(19).prop("title", Data[j].Remarks);
                        Data[j].Remarks = Data[j].Remarks.substring(0, 40) + "...";
                    }
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(19).html(Data[j].Remarks).css({ "text-align": "left" }).addClass("noExl");
                    //给第19行加样式
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(19).css({ "text-align": "left" });

                    //第20行（OriCurr_tt_Amt）特殊处理
                    if (Data[j].LoadUnit != Data[j].POUnit && Data[j].LoadUnit != null && Data[j].POUnit != null && Data[j].LoadUnit != '' && Data[j].POUnit != '') {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(20).html("N/A").addClass("changebgcolor"); //OriCurr_tt_Amt
                    }
                    else {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(20).html(Data[j].OriCurr_tt_Amt);
                    }
                    //第21行显示USDRate（美金汇率）
                    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(21).html(Data[j].USDRate);

                    //第22行（USD_Unit_Price）特殊处理
                    if (Data[j].LoadUnit != Data[j].POUnit && Data[j].LoadUnit != null && Data[j].POUnit != null && Data[j].LoadUnit != '' && Data[j].POUnit != '') {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(22).html("N/A").addClass("changebgcolor");//USD_Unit_Price
                    }
                    else {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(22).html(Data[j].USD_Unit_Price);
                    }
                    //第23行（USD_tt_Amt）特殊处理
                    if (Data[j].LoadUnit != Data[j].POUnit && Data[j].LoadUnit != null && Data[j].POUnit != null && Data[j].LoadUnit != '' && Data[j].POUnit != '') {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(23).html("N/A").addClass("changebgcolor");//USD_tt_Amt
                    }
                    else {
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(23).html(Data[j].USD_tt_Amt);
                    }
                    //显示N/A和红色字体
                    if (Data[j].LoadUnit != Data[j].POUnit && Data[j].LoadUnit != null && Data[j].POUnit != null && Data[j].LoadUnit != '' && Data[j].POUnit != '') {
                        //字体变红
                        //装箱表Unit字段
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(4).addClass("fontred");
                        //K3数据Unit字段
                        $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).addClass("fontred");
                    }


                    //判断是否出现同一采购订单多笔相同物料号（第13行）已作废
                    //if (Data.length != j + 1) {
                    //    if (Data[j].PONum == Data[j + 1].PONum && Data[j].Fnumber == Data[j + 1].Fnumber) {
                    //        //同一采购订单多笔相同物料号的单价有差异的，显示小红字 “multi-price”
                    //        if (Data[j].UnitPrice != Data[j + 1].UnitPrice) {
                    //            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(Data[j].UnitPrice + '\r\n' + "<span class='fontred' style='color: red'>multi-price</span>").css({ "text-align": "right" });//po单价//显示小红字 “multi-price”);//po单价//显示小红字 “multi-price”
                    //            $('.tablebody').eq(index).children('tr').eq(j + 1).find("td").eq(13).html(Data[j].UnitPrice + '\r\n' + "<span class='fontred' style='color: red'>multi-price</span>").css({ "text-align": "right" });//po单价//显示小红字 “multi-price”);//po单价//显示小红字 “multi-price”
                    //        }
                    //        else {
                    //            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(Data[j].UnitPrice).css({ "text-align": "right" });//po单价
                    //        }
                    //    }
                    //    else {
                    //        if ($('.tablebody').eq(index).children('tr').eq(j - 1).find("td").eq(13).html().indexOf("multi-price") != -1 && Data[j].PONum == Data[j - 1].PONum && Data[j].Fnumber == Data[j - 1].Fnumber) {
                    //            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(Data[j].UnitPrice + '\r\n' + "<span class='fontred' style='color: red'>multi-price</span>").css({ "text-align": "right" });//po单价//显示小红字 “multi-price”);//po单价//显示小红字 “multi-price”
                    //        } else {
                    //            $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(Data[j].UnitPrice).css({ "text-align": "right" });//po单价
                    //        }
                    //    }
                    //}
                    //else {
                    //    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(Data[j].UnitPrice).css({ "text-align": "right" });//po单价
                    //}
                    //将存在数据集合的数据序号加红色标注
                    //if (DataIndexList[0].find(obj => obj == Data[j].LPSerial) != undefined) {
                    //    $('.tablebody').eq(index).children('tr').eq(j).find("td").eq(13).html(Data[j].UnitPrice + '\r\n' + "<span class='fontred' style='color: red'>multi-price</span>").css({ "text-align": "right" });//po单价//显示小红字 “multi-price”);//po单价//显示小红字 “multi-price”
                    //}
                }
                if (canshu != 0) {
                    var processCount = Math.floor(yy + ((j + 1) * (100 - yy) / tempLength));
                    if (processCount >= 100) {
                        jdt(100);
                    }
                    else {
                        jdt(processCount);
                    }
                }
                j++;
                //下一步循环  
                this.window.setTimeout(loop, 0); //递归
            }
            this.loopId = window.setTimeout(loop, 0);
        }
        else {
            /*jdt(100); //进度条赋值*/
            $("#XMModal").modal('hide');
            $scope.globalmodal(false);
            //没有数据禁用开始采集按钮
            $('#two').css("pointer-events", "none").attr('disabled', "true");
            $(".tablebody").append("<tr><td colspan='20' class='text-center' style='color:red;font-size:20px'>未找到任何记录</td></tr>");
            NullData = true;
        }
    }

    $scope.List(0);

})
//格式化时间
Date.prototype.Format = function (fmt) { // author: meizz
    var o = {
        "M+": this.getMonth() + 1, // 月份
        "d+": this.getDate(), // 日
        "h+": this.getHours(), // 小时
        "m+": this.getMinutes(), // 分
        "s+": this.getSeconds(), // 秒
        "q+": Math.floor((this.getMonth() + 3) / 3), // 季度
        "S": this.getMilliseconds() // 毫秒
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}
//给进度条赋值
function jdt(processCount) {
    var processCount = processCount;
    document.haorooms.haoroomsinput0.value = processCount;
    $("#tt0").html(processCount + "%");
    $("#tt0").css("width", processCount + "%");
}
//导出表格
function newApiArray(format, username) {
    var myDate = new Date();
    var time = myDate.Format("yyyyMMddhhmm");  //获得当前年月日时分秒
    //return ExcellentExport.convert({
    //    anchor: 'anchorNewApi-' + format + '-array',
    //    filename: time + "〈" + username + "〉LoadingList w POData.xls",
    //    format: format
    //}, [{
    //    name: 'Table',
    //    from: {
    //        table: 'DaoChuGridView'
    //    }
    //}]);
    $("#GridView").table2excel({
        // 不被导出的表格行的CSS class类
        exclude: ".noExl",
        // 导出的Excel文档的名称
        name: "Excel Document Name",
        // Excel文件的名称
        filename: time + username + "LoadingList w PODate.xls"
        /* specialStyle: ["fontred", "changebgcolor"]*/
    });
}




