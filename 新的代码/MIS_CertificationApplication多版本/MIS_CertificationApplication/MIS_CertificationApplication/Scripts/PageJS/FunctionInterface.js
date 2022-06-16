var app = angular.module('myApp', []);  //创建模块
app.controller('mycontroller', function ($scope, $compile) {
    //下拉框初始化
    {
        //委任申请公司下拉框列表
        $scope.Applicant = function () {
            var Applicant = "";
            $.ajax({
                type: "post",
                dataType: 'JSON',
                url: "/CertificationApplication/CertificationApplicationSQL/GetApplicantList",
                success: function (result) {
                    $.each(result, function (key, value) {
                        Applicant += '<option value="' + value.Name + '"';
                        Applicant += ">" + value.Text + '</option > ';
                    })
                    $("#Applicant").append(Applicant);
                    //不是新增的话，给下拉框赋默认值
                    if ($("#type").val() != 1) {
                        $("#ModelCode").find("option[value='" + + "']").attr("selected", true);
                    }
                    return false;
                }
            });
        }
        $scope.Applicant();
        //制造商下拉框列表
        $scope.Manufacturer = function () {
            var Manufacturer = "";
            $.ajax({
                type: "post",
                dataType: 'JSON',
                url: "/CertificationApplication/CertificationApplicationSQL/GetManufacturerList",
                success: function (result) {
                    $.each(result, function (key, value) {
                        Manufacturer += '<option value="' + value.Name + '"';
                        Manufacturer += ">" + value.Text + '</option > ';
                    })
                    $("#Manufacturer").append(Manufacturer);
                    return false;
                }
            });
        }
        $scope.Manufacturer();
        //客户下拉框列表
        $scope.Customer = function () {
            var Customer = "";
            $.ajax({
                type: "post",
                dataType: 'JSON',
                url: "/CertificationApplication/CertificationApplicationSQL/GetCustomerList",
                success: function (result) {
                    $.each(result, function (key, value) {
                        Customer += '<option value="' + value + '"';
                        Customer += ">" + value + '</option > ';
                    })
                    $("#Customer").append(Customer);
                    return false;
                }
            });
        }
        $scope.Customer();
        //费用货币下拉框列表
        $scope.Currency = function () {
            var Currency = "";
            $.ajax({
                type: "post",
                dataType: 'JSON',
                url: "/CertificationApplication/CertificationApplicationSQL/GetCurrencyList",
                success: function (result) {
                    $.each(result, function (key, value) {
                        Currency += '<option value="' + value.Name + '"';
                        Currency += ">" + value.Text + '</option > ';
                    })
                    $("#Currency").append(Currency);
                    return false;
                }
            });
        }
        $scope.Currency();
    }

    //新增按钮点击保存数据
    $scope.InsertData = function () {
        //子表的数据
        var SonListsArray = []; //存放子表数据
        for (var i = 0; i < index; i++) {
            //取自定义属性的值
            var Name = $('#GridView').find("select option:selected").eq(i).val();
            var ReferenceNumber = $('#GridView').find("input[name='ReferenceNumber']").eq(i).val();
            var Issuer = $('#GridView').find("input[name='Issuer']").eq(i).val();
            var factory = $('#GridView').find(".factory").eq(i).attr("ArrFactories");  //子表工厂
            var CoverAreas = $('#GridView').find(".CoverAreas").eq(i).attr("ArrCovereArea");  //子表国家区域
            var date = $('#GridView').find("input[type='date']").eq(i).val(); //有效期
            var Attachment = $('#GridView').find(".Attachment").eq(i).val();
            SonListsArray.push({ "CM_Serial": Name, "Cert_Ref": ReferenceNumber, "Issuer": Issuer, "Factories": factory, "CoverAreas": CoverAreas, "Expiry": date, "CertFile": Attachment, "CertFileName": "file" + (i + 1) });
        }
        //console.log(SonListsArray);
        var form = $('#Myform').serialize();
        /* var Models = $("#Models").html();*/
        var Models = $("#Models").attr("ArrModels");
        var formData = new FormData();
        //获取页面已有的一个form表单
        var form = document.getElementById("Myform");
        //用表单来初始化
        var formData = new FormData(form); //文件上传一定要用FormData,表单文件上传的name不需要设置
        formData.append("Models", Models);
        formData.append("Son", JSON.stringify(SonListsArray));
        //注意这里是单张图片，只要了第一张
        /*        formData.append("Mainfile", $("#MainUpload").get(0).files[0]);*/
        //循环将文件添加到formData
        var input = $('input[type = "file"]');
        var SonNum = 0; //子表文件索引
        input.each(function (key, value) {
            if (value.id == "MainUpload") {  //主表标识
                formData.append("Mainfile", value.files[0]);
            }
            else {
                SonNum += 1;
                formData.append("file" + SonNum, value.files[0]);
            }

        });
        $.ajax({
            url: "/CertificationApplication/CertificationApplicationSQL/InsertData",
            type: 'post',
            dataType: 'json',
            data: formData,
            /*data: $.param({ "Son": SonListsArray, "Models": Models}) + '&' + from,*/
            /*data: $.param({ 'SonList': JSON.stringify(SonListsArray) }) + '&' + from,*/
            contentType: false, // 告诉jQuery不要去设置Content-Type请求头（文件上传必须要）
            processData: false,   // 告诉jQuery不要去处理发送的数据（文件上传必须要）
            cache: false, //是否缓存            
            success: function (result) {
                if (result != null && result.ReturnResultNum > 0) {
                    swal({
                        title: "保存成功",
                        text: "",
                        icon: "success",
                        buttons: {
                            button1: {
                                text: "确认",
                                value: true,
                            }
                        }
                    }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                        //跳转到详情页
                        window.location.href = "/CertificationApplication/CertificationApplication/Detail? ApplicationRef ='" + result.ApplicationRef + '';
                    });
                }
                else {
                    swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                }
            },
            error: function (res) {
                debugger;
                swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
            }
        });
    }

    //主表中多选框-模型列表的值
    $scope.SelectModels = function () {
        layer.open({
            type: 2,
            title: 'Select Products Models',
            skin: 'layui-layer-rim', //加上边框
            area: ['940px', '50%'], //宽高
            content: '../CertificationApplication/SelectModels',
            btn: ['Clear All Selected', 'Confirm & Return', 'Update Table'],
            btnAlign: 'c',
            btn1: function (index, layero) { //清楚所有的选中

            },
            btn2: function (index, layero) { //提交并返回
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为edit_category的元素，并获取其值赋值给g
                var arr = "";
                var ArrModels = ""; //存放子表中多选框-模型列表的值
                let g = body.find('input[name="test"]:checked').each(function () {
                    arr += $(this).val() + ";";
                    ArrModels += this.id + "|";  //存入数据库的
                })
                if (ArrModels.length > 0) {
                    ArrModels = ArrModels.substring(0, ArrModels.length - 1);
                }
                //给自定义属性赋值
                $("#Models").attr("ArrModels", ArrModels);
                $("#Models").html(arr);
                //关闭该子页面
                layer.close(index);
            },
            btn3: function (index, layero) { //更新表格

            },
            success: function (layero, index) {
                /*debugger*/
                //渲染按钮的样式
                $(".layui-layer-btn0").addClass("SelectModels");
                $(".layui-layer-btn1").addClass("SelectModels");
                $(".layui-layer-btn2").addClass("SelectModels");

            }
        });

    }

    //子表方法整合
    {    //用于计算子表table有几行
        var index = 0;
        //子表新增一行数据
        $scope.SonAddTr = function () {
            //动态创建tr
            var tr = '<tr>';

            tr += '<td><select name="Name" id="Name' + (index + 1) + '" class="form-control Name" ><option selected="selected" value="" style="text-align:center" ></option></select ></td>';

            tr += '<td><input name="ReferenceNumber" type="text" id="ReferenceNumber" class="form-control input-content" maxlength="30"/></td >';

            tr += '<td ><input name="Issuer" type="text" id="Issuer" class="form-control input-content" maxlength="100"/></td >';

            tr += '<td style="position:relative"><div style="margin-right:40px" class="factory"></div><a  href="" style="position:absolute;right:10px;top:10px;" ng-click="SelectFactories(' + index + ')"><img  src="../Scripts/img/factory-muliSelect.png" /></a></td >';

            tr += '<td style="position:relative"><div style="margin-right:40px" class="CoverAreas" ></div><a href="" style="position:absolute;right:10px;top:10px;" ng-click="SelectCovereArea(' + index + ')" ><img  src="../Scripts/img/CoverAreas-muilSelect.png" /></a></td >';

            tr += '<td style="position:relative"><div style="margin-right:40px" class="date" ></div><input type="date" class="form-control"></td >';
            //子表文件上传
            /* tr += '<td style="position:relative"><div style="margin-right:40px" class="Attachment" ></div><a href="" id="upload" ng-click="upLoad()" style="position:absolute;right:10px;top:10px;" ><img src="../Scripts/img/upload.png" /></a><a href="" id="delete" ng-click="delete()" style="position:absolute;right:10px;top:10px;display:none"><img src="../Scripts/img/Del.png" /></a></td >';*/

            tr += '<td style="position:relative"><div id="SonFileDiv' + (index + 1) + '" style="height: auto; width: 100px;position: relative"><div style = "position: absolute; top: 3px; right: 5px; cursor: pointer; " ><img id="uploadImg' + (index + 1) + '" src = "../Scripts/img/upload.png" title = "点击按钮上传文件" onclick = "imgclick(\'MainUpload' + (index + 1) + '\')" style = "width: 100 % " > <input id="MainUpload' + (index + 1) + '" type="file" class="file" onchange="upload(this,\'MainUpload' + (index + 1) + '\',\'uploadImg' + (index + 1) + '\',\'deleteImg' + (index + 1) + '\',\'QuoteFile' + (index + 1) + '\',\'FileDiv' + (index + 1) + '\',\'FileName' + (index + 1) + '\')" style="display: none"></div><img id="QuoteFile' + (index + 1) + '" style = "padding: 5px; display: none" /> '

            //存放文件名，没有可展示的类型图片时使用
            tr += '<div><label id="FileName' + (index + 1) + '" style="display:none"></label><a href="" id="deleteImg' + (index + 1) + '"onclick="deletefile(\'MainUpload' + (index + 1) + '\',\'uploadImg' + (index + 1) + '\',\'deleteImg' + (index + 1) + '\',\'QuoteFile' + (index + 1) + '\',\'FileDiv' + (index + 1) + '\',\'FileName' + (index + 1) + '\')" style="position: absolute; bottom:5px; right: 5px;display:none"> <img src="../Scripts/img/Del.png" title="删除已上传的文件" /> </a></div></div></td >';

            tr += '</tr>';
            index++;
            var template = angular.element(tr);
            var mobileDialogElement = $compile(template)($scope);
            angular.element("#Content").append(mobileDialogElement);
            /*$("#Content").append(tr);*/
            //$scope.SelectFactories();
            $scope.CertificateName("Name" + index);  //因为上面index已经++了，所以不用再+1了
        }

        //子表中 认证名称 下拉框列表
        $scope.CertificateName = function (Name) {
            var CertificateName = "";
            $.ajax({
                type: "post",
                dataType: 'JSON',
                url: "/CertificationApplication/CertificationApplicationSQL/GetCertificateNameList",
                success: function (result) {
                    $.each(result, function (key, value) {
                        CertificateName += '<option value="' + value.CMSerial + '"';
                        CertificateName += ">" + value.Text + '</option > ';
                    })
                    $("#" + Name).append(CertificateName);
                    return false;
                }
            });
        }

        //子表中多选框-工厂
        $scope.SelectFactories = function (element) {
            layer.open({
                type: 2,
                title: '多选GIP工厂',
                skin: 'layui-layer-rim', //加上边框
                area: ['450px', '30%'], //宽高
                content: '../CertificationApplication/SelectFactories',
                btn: ['Confirm & Return'],
                btnAlign: 'c',
                btn1: function (index, layero) { //提交并返回
                    //获取子页面（iframe页）的body元素
                    var body = layer.getChildFrame('body', index);
                    // 得到找到body元素中id为edit_category的元素，并获取其值赋值给g
                    var arr = "";
                    var ArrFactories = ""; //存放子表中多选框-工厂的值
                    let g = body.find('input[name="test"]:checked').each(function () {
                        arr += $(this).val() + ";";
                        ArrFactories += this.id + "|";  //存入数据库的
                    })
                    if (ArrFactories.length > 0) {
                        ArrFactories = ArrFactories.substring(0, ArrFactories.length - 1);
                    }
                    //给自定义属性赋值
                    $(".factory").eq(element).attr("ArrFactories", ArrFactories);
                    $(".factory").eq(element).html(arr);
                    //关闭该子页面
                    layer.close(index);
                },
                success: function (layero, index) {
                    /*debugger*/
                    //渲染按钮的样式
                    $(".layui-layer-btn0").addClass("SelectFactories");

                }
            });
        }

        //子表中多选框-国家区域
        $scope.SelectCovereArea = function (element) {
            layer.open({
                type: 2,
                title: '多 选 国 家 区 域',
                skin: 'layui-layer-rim', //加上边框
                area: ['1600px', '65%'], //宽高
                content: '../CertificationApplication/SelectCountriesAreas',
                btn: ['Clear All Selected', 'Confirm & Return'],
                btnAlign: 'c',
                btn1: function (index, layero) {

                },
                btn2: function (index, layero) { //提交并返回

                    //获取子页面（iframe页）的body元素
                    var body = layer.getChildFrame('body', index);
                    // 得到找到body元素中id为edit_category的元素，并获取其值赋值给g
                    var arr = "";
                    var ArrCovereArea = ""; //存放子表中多选框 - 国家区域的值
                    let g = body.find('input[name="test"]:checked').each(function () {
                        arr += $(this).val() + ";";
                        ArrCovereArea += this.id + "|";  //存入数据库的
                    })
                    if (ArrCovereArea.length > 0) {
                        ArrCovereArea = ArrCovereArea.substring(0, ArrCovereArea.length - 1);
                    }
                    //给自定义属性赋值
                    $(".CoverAreas").eq(element).attr("ArrCovereArea", ArrCovereArea);
                    $(".CoverAreas").eq(element).html(arr);
                    //关闭该子页面
                    layer.close(index);
                },
                success: function (layero, index) {
                    /* debugger*/
                    //渲染按钮的样式
                    $(".layui-layer-btn0").addClass("SelectCovereArea");
                    $(".layui-layer-btn1").addClass("SelectCovereArea");

                }
            });
        }
    }
})

//文件上传
{
    //点击图片文件上传
    function imgclick(MainUploadid) {
        $("#" + MainUploadid).click();
    }
    /**
     * 文件上传
     * @param target  对象本身
     * @param uploadid  上传的input的id
     * @param uploadImgid   上传图标的id
     * @param deleteImgid   删除图标的id
     * @param QuoteFileid   上传完文件后显示的图片id
     * @param FileDivid   上传文件所在div的id
     * @param FileName   未找到匹配的文件名label标签id
     */
    function upload(target, uploadid, uploadImgid, deleteImgid, QuoteFileid, FileDivid, FileName) {
        //var files = e.target.files;
        //得到上传文件的值
        var fileName = document.getElementById(uploadid).value;
        //返回String对象中子字符串最后出现的位置.
        var seat = fileName.lastIndexOf(".") + 1;
        //返回位于String对象中指定位置的子字符串并转换为小写.
        var extension = fileName.substring(seat).toLowerCase();
        //查询文件后缀名是否符合上传档案格式标准
        //if ("csv" != extension) {
        //    swal('只能上传后缀名为csv的文件！', '', 'error');
        //    return false;
        //}
        //文件是否超过5M
        var fileSize = 0;
        fileSize = target.files[0].size / 1024; //换算成kb
        if (fileSize > 5120) {
            swal('档案超出5M限制,请选择正确的文件重新上传！', '', 'error');
            return false;
        }
        if (IsUploadStandard(extension) == 0) {
            swal('档案类型不允许,请选择正确的文件重新上传！', '', 'error');
            return false;
        }
        //显示上传的文件图片在页面上
        $("#" + QuoteFileid).css({ 'display': 'block' }).prop("title", target.files[0].name);
        //匹配文件类型图片
        if (extension == "pdf") {
            document.getElementById(QuoteFileid).src = "../Scripts/img/pdf.png";
        }
        else if (extension == "doc" || extension == "docx") {
            document.getElementById(QuoteFileid).src = "../Scripts/img/word.png";
        }
        else if (extension == "xls" || extension == "xlsx" || extension == "csv") {
            document.getElementById(QuoteFileid).src = "../Scripts/img/excel.png";
        }
        else if (extension == "ppt" || extension == "pptx") {
            document.getElementById(QuoteFileid).src = "../Scripts/img/ppt.png";
        }
        else {
            //隐藏可以匹配的图片
            $("#" + QuoteFileid).css({ 'display': 'none' });
            //未找到匹配的把文件名显示出来
            $("#" + FileName).css({ 'display': 'block' }).html(target.files[0].name);
        }
        //隐藏上传图标
        $("#" + uploadImgid).css({ 'display': 'none' });
        //显示删除图标在页面上
        $("#" + deleteImgid).css({ 'display': 'block' });
        $("#" + FileDivid).css({ 'width': 'auto', 'padding': '5px 40px 5px 5px ' });
    }
    //查询文件后缀名是否符合上传档案格式标准
    function IsUploadStandard(FileSuffix) {
        $.ajax({
            type: "post",
            dataType: 'JSON',
            url: "/CertificationApplication/CertificationApplicationSQL/IsUploadStandard",
            data: { "FileSuffix": FileSuffix },
            success: function (result) {
                return result;
            }
        });
    }
    /**
     * 删除按钮
     * @param uploadImgid   上传图标的id
     * @param deleteImgid   删除图标的id
     * @param QuoteFileid   上传完文件后显示的图片id
     * @param FileDivid   上传文件所在div的id
     * @param FileName   未找到匹配的文件名label标签id
     */
    function deletefile(MainUploadid, uploadImgid, deleteImgid, QuoteFileid, FileDivid, FileName) {
        //隐藏删除图标
        $("#" + deleteImgid).css({ 'display': 'none' });
        //显示上传图标在页面上
        $("#" + uploadImgid).css({ 'display': 'block' });
        //隐藏上传的文件图片
        $("#" + QuoteFileid).css({ 'display': 'none' });
        //重置div
        $("#" + FileDivid).css({ 'width': '100px', 'padding': '' });
        //隐藏文件名
        $("#" + FileName).css({ 'display': 'none' });
        //清空文件
        $("#" + MainUploadid).val("");
    }
}

//循环表单（已作废）
function getform() {
    var form1 = document.getElementById("Myform");
    for (i = 0; i < form1.elements.length; i++) {
        var e = form1.elements[i];
        //alert(e.name + "\t" + e.value);
        e.name = "" + e.name + "";
        if (e.value == "") {
            e.value = null;
        }
        if (e.name != "QuoteFile") {
            /* formData.append(e.name, e.value);*/
        }
        /*formData.append("", "");*/
    }
    console.log(jsonObj);
}

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
