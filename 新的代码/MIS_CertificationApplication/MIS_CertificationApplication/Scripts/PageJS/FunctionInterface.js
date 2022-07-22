var app = angular.module('myApp', []);  //创建模块
app.controller('mycontroller', function ($scope, $compile) {
    /**
    *新增/复制/修改
    * @param type 1-新增，2-复制，3-修改
    * @param ApplicationRef  复制和修改才有ApplicationRef(认证申请编号)
     */
    $scope.OperationData = function (type, ApplicationRef) {
        //Customer客户下拉框验证
        if ($("#Customer").val() == "") {
            swal({
                title: "保存失败",
                text: "Customer下拉框未挑选，请挑选Customer后再点击Confirm按钮",
                type: "error",
                buttons: {
                    button1: {
                        text: "确认"
                    }
                }
            }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                $("#Customer").focus();
            });
            return false;
        }
        //Models验证非空
        if ($("#Models").html() == "") {
            swal('保存失败!', '原因：Product Model未挑选，请挑选Product Model后再点击Confirm按钮', 'error') //提示框
            return false;
        }
        //子表的数据
        var SonListsArray = []; //存放子表数据
        if (type != 1) { //复制和修改用
            index = $("#Content tr").length;
        }
        for (var i = 0; i < index; i++) {
            //取自定义属性的值
            var Name = $('#GridView').find("select option:selected").eq(i).val();
            //认证名称下拉框非空验证
            if (Name == "") {
                swal({
                    title: "保存失败",
                    text: "子表的Name下拉框未挑选，请挑选Name后再点击Confirm按钮",
                    type: "error",
                    buttons: {
                        button1: {
                            text: "确认"
                        }
                    }
                }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                    $("#Name" + (i + 1)).focus();
                });
                return false;
            }
            var ReferenceNumber = $('#GridView').find("input[name='ReferenceNumber']").eq(i).val();
            var Issuer = $('#GridView').find("input[name='Issuer']").eq(i).val();
            var factory = $('#GridView').find(".factory").eq(i).attr("ArrFactories");  //子表工厂
            var CoverAreas = $('#GridView').find(".CoverAreas").eq(i).attr("ArrCovereArea");  //子表国家区域
            var date = $("#date" + i).val(); //有效期
            var Attachment = $('#GridView').find(".Attachment").eq(i).val();
            var Status = "";
            //修改需要传状态
            if (type == 3) {
                Status = $("tbody").children().eq(i).children().eq(7).find("label").text(); //状态
                if (Status == "Pending") {
                    Status = "P";
                }
                else if (Status == "Active") {
                    Status = "A";
                }
                else if (Status == "Expired") {
                    Status = "E";
                }
                else if (Status == "Discard") {
                    Status = "D";
                }
            }
            SonListsArray.push({ "CM_Serial": Name, "Cert_Ref": ReferenceNumber, "Issuer": Issuer, "Factories": factory, "CoverAreas": CoverAreas, "Expiry": date, "CertFile": Attachment, "CertFileName": "file" + (i + 1), "Sonflag": true, "Status": Status });
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
        if (ApplicationRef != "") { //复制和修改才有ApplicationRef(认证申请编号)
            formData.append("CA_Ref", ApplicationRef);
        }
        //循环将文件添加到formData
        var input = $('input[type = "file"]');
        var SonNum = 0; //子表文件索引
        input.each(function (key, value) {
            if (value.id == "MainUpload") {//主表标识
                formData.append("Mainfile", value.files[0]);
                //true取旧文件，false取新文件
                if ($("#" + value.id).attr("QuoteFileFlag") != "0") {
                    formData.append("Mainflag", true);
                }
                else {
                    formData.append("Mainflag", false);
                }
            }
            else {
                if ($("#" + value.id).attr("CertFileFlag") != "0") {
                    SonListsArray[SonNum].Sonflag = true; //还是旧文件
                }
                else {
                    SonListsArray[SonNum].Sonflag = false;  //上传了新文件
                }
                SonNum += 1;
                formData.append("file" + SonNum, value.files[0]);
            }

        });
        formData.append("Son", JSON.stringify(SonListsArray));
        var method = "";
        if (type == 2 || type == 3) {
            formData.append("type", type);
            method = "CopyOrUpdateData"; //复制/修改
        }
        else {
            method = "InsertData"; //新增
        }
        $.ajax({
            async: false,
            url: "/CertificationApplication/CertificationApplicationSQL/" + method,
            type: 'post',
            dataType: 'json',
            data: formData,
            contentType: false, // 告诉jQuery不要去设置Content-Type请求头（文件上传必须要）
            processData: false,   // 告诉jQuery不要去处理发送的数据（文件上传必须要）
            cache: false, //是否缓存            
            success: function (result) {
                if (result != null && result.ReturnResultNum > 0) {
                    swal({
                        title: "保存成功",
                        text: "",
                        type: "success",
                        buttons: {
                            button1: {
                                text: "确认",
                                value: result.ApplicationRef
                            }
                        }
                    }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                        //跳转到详情页
                        window.location.href = "/CertificationApplication/CertificationApplication/Detail?ApplicationRef=" + result.ApplicationRef;
                        //测试暂用
                        /*location.reload();*/
                    });
                }
                else {
                    swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                }
            },
            error: function (e) {
                console.log(e);
                debugger;
                swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
            }
        });
    }

    //主表中多选框-模型列表的值
    $scope.SelectModels = function () {
        var Models = $("#Models").attr("arrModels");
        layer.open({
            type: 2,
            title: 'Select Products Models',
            skin: 'layui-layer-rim', //加上边框
            area: ['940px', '50%'], //宽高
            content: '../CertificationApplication/SelectModels?Models=' + Models,
            btn: ['Clear All Selected', 'Confirm & Return'/*, 'Update Table'*/],
            btnAlign: 'c',
            btn1: function (index, layero) { //清楚所有的选中
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                body.find("input[name='test']").each(function () {
                    $(this).removeAttr("checked");
                });
            },
            btn2: function (index, layero) { //提交并返回
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为edit_category的元素，并获取其值赋值给g
                var arr = "";
                var isnull = "";
                var ArrModels = ""; //存放子表中多选框-模型列表的值
                let g = body.find('input[name="test"]:checked').each(function () {
                    isnull += $(this).val();
                    arr += $(this).val() + ";";
                    ArrModels += this.id + "|";  //存入数据库的
                })
                if (ArrModels.length > 0) {
                    ArrModels = ArrModels.substring(0, ArrModels.length - 1);
                }
                //给自定义属性赋值
                $("#Models").attr("ArrModels", ArrModels);
                $("#Models").html(arr);
                if (isnull != "") {
                    //改变标题颜色和背景颜色
                    $("#ProductModellabel").css({ "color": "black", "background-color": "lightgray" });
                }
                else {
                    $("#ProductModellabel").css({ "color": "white", "background-color": "red" });
                }
                //关闭该子页面
                layer.close(index);
            },
            //btn3: function (index, layero) { //更新表格

            //},
            success: function (layero, index) {
                /*debugger*/
                //渲染按钮的样式
                $(".layui-layer-btn0").addClass("SelectModels");
                $(".layui-layer-btn1").addClass("SelectModels");
                $(".layui-layer-btn2").addClass("SelectModels");
            }
        });
    }

    //主表中  新增模型  图标按钮的点击事件
    $scope.ProductModelUpdate = function () {
        layer.open({
            type: 2,
            title: '元  器  件 型 号 管 理',
            skin: 'layui-layer-rim', //加上边框
            area: ['98%', '95%'], //宽高
            content: '../CertificationApplication/Product_Model_Update',
            btnAlign: 'c'
        });
    }

    //子表方法整合
    {    //全局变量-用于计算子表table有几行
        var index = 0;

        //子表新增一行数据
        $scope.SonAddTr = function (type) {
            if (type != 1) { //复制和修改用
                index = $("#Content tr").length;
            }
            //动态创建tr
            var tr = '<tr>';
            //认证名称下拉框
            tr += '<td><select name="SonName" onchange="CheckReferenceNumberAndSonName(this,' + (index + 1) + ')" id="SonName' + (index + 1) + '" class="form-control Name" ><option selected="selected" value="" style="text-align:center" ></option></select ></td>';
            //Reference Number文本框
            tr += '<td><input name="ReferenceNumber" type="text" id="ReferenceNumber' + (index + 1) + '" class="form-control input-content" maxlength="30" onblur="CheckReferenceNumberAndSonName(this,' + (index + 1) + '),ChangeStatus(' + index + ')"/></td >';
            //Issuer文本框
            tr += '<td ><input name="Issuer" type="text" id="Issuer" class="form-control input-content" maxlength="100"/></td >';
            //factory多选框
            tr += '<td style="position:relative"><div style="margin-right:40px" class="factory"></div><a  href="" style="position:absolute;right:10px;top:10px;" ng-click="SelectFactories(' + index + ')"><img  src="../Scripts/img/factory-muliSelect.png" /></a></td >';
            //CoverAreas多选框
            tr += '<td style="position:relative"><div style="margin-right:40px" class="CoverAreas" ></div><a href="" style="position:absolute;right:10px;top:10px;" ng-click="SelectCovereArea(' + index + ')" ><img  src="../Scripts/img/CoverAreas-muilSelect.png" /></a></td >';
            //有效期日期选择器
            tr += '<td><div style="position:relative"><input type="text" name="date" id="date' + index + '" lay-verify="date" autocomplete="off" class="layui-input" style="cursor:pointer"><i class="DateIcon"><img src="../Scripts/img/日期选择器Icon.png" /></i></div></td >';
            //子表文件上传
            /* tr += '<td style="position:relative"><div style="margin-right:40px" class="Attachment" ></div><a href="" id="upload" ng-click="upLoad()" style="position:absolute;right:10px;top:10px;" ><img src="../Scripts/img/upload.png" /></a><a href="" id="delete" ng-click="delete()" style="position:absolute;right:10px;top:10px;display:none"><img src="../Scripts/img/Del.png" /></a></td >';*/

            tr += '<td style="position:relative"><div id="SonFileDiv' + (index + 1) + '" style="height: auto; width: 100px"><div style = "position: absolute; top: 3px; right: 5px; cursor: pointer; " ><img id="uploadImg' + (index + 1) + '" src = "../Scripts/img/upload.png" title = "点击按钮上传文件" onclick = "imgclick(\'MainUpload' + (index + 1) + '\')" style = "width: 100 % " > <input id="MainUpload' + (index + 1) + '" type="file" class="file" CertFileFlag="0" onchange="upload(' + index + ',' + type + ',this,\'MainUpload' + (index + 1) + '\',\'uploadImg' + (index + 1) + '\',\'deleteImg' + (index + 1) + '\',\'QuoteFile' + (index + 1) + '\',\'FileDiv' + (index + 1) + '\',\'FileName' + (index + 1) + '\')" style="display: none"></div><img id="QuoteFile' + (index + 1) + '" style = "padding: 5px; display: none" /> '

            //存放文件名，没有可展示的类型图片时使用
            tr += '<div><label id="FileName' + (index + 1) + '" style="display:none;"></label><a href="" id="deleteImg' + (index + 1) + '"onclick="deletefile(\'MainUpload' + (index + 1) + '\',\'uploadImg' + (index + 1) + '\',\'deleteImg' + (index + 1) + '\',\'QuoteFile' + (index + 1) + '\',\'FileDiv' + (index + 1) + '\',\'FileName' + (index + 1) + '\'),ChangeStatus(' + index + ')" style="position: absolute; bottom:20px; right: 10px;display:none"> <img src="../Scripts/img/Del.png" title="删除已上传的文件" /> </a></div></div></td >';

            //修改需要新增状态
            if (type == 3) {
                tr += '<td style="color:green"><label></label><br/>'
                tr += '<div style="display:contents">'
                tr += '<a href="" id="cxl' + (index + 1) + '" title="作废"  ng-click="CXL(' + index + ')"><img src = "../Scripts/img/cxl.png" /></a>'
                tr += '<a href="" id="Cancelcxl' + (index + 1) + '" title="取消作废"  ng-click="CancelCXL(' + index + ',$event)" style="pointer-events: auto !important; display: none"><img src = "../Scripts/img/cxl.取消作废" /></a>'
                tr += '</div >'
                tr += '</td >'
            }
            tr += '</tr>';
            var template = angular.element(tr);
            var mobileDialogElement = $compile(template)($scope);
            angular.element("#Content").append(mobileDialogElement);
            /*$("#Content").append(tr);*/
            //$scope.SelectFactories();
            //时间控件
            //日期
            layui.use('laydate', function () {
                var laydate = layui.laydate;
                var Newdate = "#date" + index;
                var num = index;
                //常规用法
                laydate.render({
                    elem: Newdate,
                    done: function (date) {
                        ChangeStatus(num);
                    }
                });
                index++;
            });

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
                    $("Select[name='SonName']").append(CertificateName);
                    return false;
                }
            });
        }

        //子表中多选框-工厂
        $scope.SelectFactories = function (element) {
            var Factories = $('#GridView').find(".factory").eq(element).attr("ArrFactories");  //子表工厂
            layer.open({
                type: 2,
                title: '多选GIP工厂',
                skin: 'layui-layer-rim', //加上边框
                area: ['450px', '35%'], //宽高
                content: '../CertificationApplication/SelectFactories?Factories=' + Factories,
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
            var CoverAreas = $('#GridView').find(".CoverAreas").eq(element).attr("ArrCovereArea");  //子表国家区域
            layer.open({
                type: 2,
                title: '多 选 国 家 区 域',
                skin: 'layui-layer-rim', //加上边框
                area: ['1600px', '65%'], //宽高
                content: '../CertificationApplication/SelectCountriesAreas?CoverAreas=' + CoverAreas,
                btn: ['Clear All Selected', 'Confirm & Return'],
                btnAlign: 'c',
                btn1: function (index, layero) {
                    //获取子页面（iframe页）的body元素
                    var body = layer.getChildFrame('body', index);
                    body.find("input[name='test']").each(function () {
                        $(this).removeAttr("checked");
                    });
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
                    $(".layui-layer-btn0").addClass("SelectcovereArea");
                    $(".layui-layer-btn1").addClass("SelectcovereArea");
                }
            });
        }

        //子表中认证名称操作
        $scope.CertificatesManagement = function () {
            //循环存选中值到数组里
            var SonArr = [];
            $.each($("select[name='SonName']"), function (key, value) {
                SonArr.push($(this).val());
            })
            layer.open({
                type: 2,
                title: '认 证 管 理',
                skin: 'layui-layer-rim', //加上边框
                area: ['98%', '95%'], //宽高
                content: '../CertificationApplication/Certificate_Update',
                btnAlign: 'c',
                end: function () {
                    //循环加载下拉框列表
                    var num = 1;
                    for (var i = 0; i < SonArr.length; i++) {

                        $scope.CertificateName("Name" + num);
                        num++;
                        ////赋默认值
                        //$("#Name" + i).find("option['value=\"" + SonArr[i] + "\"']").attr("selected", "selected");
                    }
                }
            });

        }

        //子表中 作废操作
        $scope.CXL = function (id) {
            swal({
                title: '您确定是否作废此条数据?',
                text: "",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: '确定',
                cancelButtonText: '取消'
            }).then((isConfirmed) => {
                if (isConfirmed) {
                    $("tbody").children().eq(id).css("pointer-events", "none");//禁用本条数据  当前tr
                    //打开取消作废按钮的td
                    $("tbody").children().eq(id).children().eq(7).css("pointer-events", "auto");//开启本条数据的功能按钮  当前tr
                    $("tbody").children().eq(id).css("background-color", "rgb(255,233,233)");//禁用本条数据后  将tr背景颜色改为红色
                    $("tbody").children().eq(id).children().eq(1).children().val("");//清空认证编号
                    $("tbody").children().eq(id).children().eq(7).css("color", "orange");//将作废状态字体颜色改为orange
                    $("tbody").children().eq(id).children().eq(7).find("label").html("Discard");//作废后 将状态改为Discard
                    //取消作废按钮显示，作废按钮隐藏
                    //$("#Cancelcxl" + (2)).css({ "display": "block" });
                    $("#Cancelcxl" + (2)).children().css({ "display": "inline" });
                    $("#cxl" + (2)).css({ "display": "none" });
                    //$(MyEvent.target).parent().siblings().css({ "display": "block" });
                    //$(MyEvent.target).css({ "display": "none" });
                }
            })
        }

        //子表中 取消作废操作
        $scope.CancelCXL = function (id, MyEvent) {
            //恢复功能
            $("tbody").children().eq(id).css("pointer-events", "auto");//开启本条数据的功能按钮  当前tr
            $("tbody").children().eq(id).css("background-color", "white");//禁用本条数据后  将tr背景颜色改为红色
            //作废按钮显示，取消作废按钮隐藏
            $(MyEvent.target).parent().siblings().css({ "display": "block" });
            $(MyEvent.target).css({ "display": "none" });
            //更新状态
            ChangeStatus(id);
        }
    }
})

//货币失焦
function FeeStyle(NameStr, id) {
    if (NameStr.indexOf('.') != -1) {
        var str = NameStr.split('.');
        var arr = "";
        var len = str[1].length;
        if (len == 1) {
            str[1] = str[1] + "0";
        }
        arr = str[0] + '.' + str[1];
        $("#" + id).val(arr);
    } else {
        if (NameStr != "") {
            $("#" + id).val(NameStr + ".00");
        } else {
            $("#" + id).val(NameStr + '0.00');
        }

    }
}
//货币聚焦
function FeeFucsStyle(NameStr, id) {
    if (NameStr.indexOf('.') == 1) {
        $("#" + id).val("");
    }
}

//文件上传
{
    //点击图片文件上传
    function imgclick(MainUploadid, type, downloadFileid, QuoteFileid) {
        //修改界面需要将a标签去除
        if (type == 2 || type == 3) {
            //after-在指定标签之后添加新的标签
            $("#" + downloadFileid).after('<img id=' + QuoteFileid + ' style="padding: 5px; display: none" />');
            $("#" + downloadFileid).remove();
        }
        $("#" + MainUploadid).click();
    }
    /**
     * 文件上传
     * @param index  当前tr
     * @param type  1-新增，2-复制，3-修改
     * @param target  对象本身
     * @param uploadid  上传的input的id
     * @param uploadImgid   上传图标的id
     * @param deleteImgid   删除图标的id
     * @param QuoteFileid   上传完文件后显示的图片id
     * @param FileDivid   上传文件所在div的id
     * @param FileName   未找到匹配的文件名label标签id
     */
    function upload(index, type, target, uploadid, uploadImgid, deleteImgid, QuoteFileid, FileDivid, FileName) {
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
        $("#" + QuoteFileid).css({ 'display': 'inline-block' }).prop("title", target.files[0].name);
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
            $("#" + FileName).css('display', 'block').html("其他类型");
            $("#" + FileName).attr("title", target.files[0].name);
        }
        //隐藏上传图标
        $("#" + uploadImgid).css({ 'display': 'none' });
        //显示删除图标在页面上
        $("#" + deleteImgid).css({ 'display': 'inline-block' });
        $("#" + FileDivid).css({ 'width': '120px', 'padding': '5px 40px 5px 5px ' });
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
        $("#" + uploadImgid).css({ 'display': 'inline-block' });
        //隐藏上传的文件图片
        $("#" + QuoteFileid).css({ 'display': 'none' });
        //重置div
        $("#" + FileDivid).css({ 'width': '100px', 'padding': '' });
        //隐藏文件名
        $("#" + FileName).css({ 'display': 'none' });
        //清空文件
        $("#" + MainUploadid).val("");
        //改变input自定义属性的值为0,代表不需要旧文件
        //代表主表
        if ($("#" + MainUploadid).attr("id") == "MainUpload") {
            $("#" + MainUploadid).attr("QuoteFileFlag", 0);
        }
        else {
            $("#" + MainUploadid).attr("CertFileFlag", 0);
        }
    }
}

//文件下载
{
    /**
        * 下载
        * @param type  //0-主表，1-子表
        * @param node   //本身
        * @param fileName   //文件名称
        * @param Primarykey   //主键
        */
    function downloadFile(type, node, fileName, Primarykey) {
        var hzm = fileName.substring(fileName.lastIndexOf(".") + 1, fileName.length);
        var contentType = "application/" + hzm;
        /*    Content += "data:base64," + Content;*/
        //0-主表，1-子表
        var Content = GetFileByPrimarykey(type, Primarykey); //文件
        if (Content != "0") {
            let aLink = document.getElementById(node.id);
            let blob = base64ToBlob(Content, contentType); //new Blob([Content]);
            aLink.download = fileName;
            aLink.href = URL.createObjectURL(blob);
        }
        else {
            return false;
        }
    };
    /**
         * base64转blob
         * @param code  //base64码
         * @param contentType   //文件类型
         */
    function base64ToBlob(code, contentType) {
        let raw = window.atob(code);
        let rawLength = raw.length;
        let uInt8Array = new Uint8Array(rawLength);
        for (let i = 0; i < rawLength; ++i) {
            uInt8Array[i] = raw.charCodeAt(i);
        }
        return new Blob([uInt8Array], { type: contentType });
    }

    //通过主键获取文件及其文件名
    function GetFileByPrimarykey(type, Primarykey) {
        var sqlurl = "";
        if (type == 0) {
            //主表
            sqlurl = "GetQuoteFileByCA_Ref?CA_Ref=" + Primarykey;
        }
        if (type == 1) {
            //子表
            sqlurl = "GetCertFileByCFSerial?CFSerial=" + Primarykey;
        }
        var file = "";
        //根据子表主键查对应的文件
        $.ajax({
            async: false,
            type: "post",
            dataType: 'JSON',
            url: "/CertificationApplication/CertificationApplicationSQL/" + sqlurl,
            success: function (result) {
                if (result == null || result.FileBase64 == null) {
                    return 0;
                }
                else {
                    file = result.FileBase64;
                }

            },
            error: function (e) {
                swal('操作失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                return 0;
            }
        });
        return file;
    }

    //查询文件后缀名是否符合上传档案格式标准
    function IsUploadStandard(FileSuffix) {
        var res = 0;
        $.ajax({
            async: false,
            type: "post",
            url: "/CertificationApplication/CertificationApplicationSQL/IsUploadStandard",
            data: { "FileSuffix": FileSuffix },
            success: function (result) {
                if (result == -1) {
                    swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                    return false;
                }
                else {
                    res = result;
                }

            }
        });
        return res;
    }
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

//验证方法整合
{
    //做必填选项判断
    function FeiKong() {
        if ($("#Customer").val() == "") {
            //改变标题颜色和背景颜色
            $("#Customerlabel").css({ "color": "white", "background-color": "red" });
        }
        else {
            //改变标题颜色和背景颜色
            $("#Customerlabel").css({ "color": "black", "background-color": "lightgray" });
        }
    }

    //做验证改变状态：子表认证名称失焦事件/日期选择器改变事件/文件上传事件/取消作废操作更新状态
    function ChangeStatus(index) {
        var num;
        //判断当前数据是否有文件
        $("#MainUpload" + (parseInt(index) + 1)).each(function (key, value) {
            CertFileFlag = $("#MainUpload" + (parseInt(index) + 1)).attr("CertFileFlag");
            if (CertFileFlag == "0" && value.files[0] == null) {
                num = 0;  //没有文件
            }
            else {
                num = 1;
            }
        })
        //在没有认证编号和文件的条件下，就不用分“过期 / 有效”了,直接改为在办
        var ReferenceNumber = $("tbody").children().eq(index).children().eq(1).find("input").val(); //认证编号
        if (ReferenceNumber == "" && num == 0) {
            $("tbody").children().eq(index).children().eq(7).css("color", "blue");
            $("tbody").children().eq(index).children().eq(7).find("label").html("Pending");
            return false;
        }
        else {
            var dateold = $("tbody").children().eq(index).children().eq(5).find("input").val(); //有效期
            if (dateold != "") {
                var date = new Date(dateold);
                var nowDate = new Date();
                if (date < nowDate) {
                    $("tbody").children().eq(index).children().eq(7).css("color", "red");
                    $("tbody").children().eq(index).children().eq(7).find("label").html("Expired");
                }
                else {
                    $("tbody").children().eq(index).children().eq(7).css("color", "green");//将作废状态字体颜色改为green
                    $("tbody").children().eq(index).children().eq(7).find("label").html("Active");//作废后 将状态改为Active
                }
            }
            else {
                //未选日期默认为永久有效
                $("tbody").children().eq(index).children().eq(7).css("color", "green");//将作废状态字体颜色改为green
                $("tbody").children().eq(index).children().eq(7).find("label").html("Active");//作废后 将状态改为Active
            }
        }
    }

    //检查 相同的认证名称，认证编号不能相同
    function CheckReferenceNumberAndSonName(element, index) {
        var ReferenceNumber = $("#ReferenceNumber" + index).val();
        var Name = $("#SonName" + index).val();
        var num = $("#Content tr").length;
        //认证名称为空就跳过判断
        if (Name == "") {
            $("#SonName" + index).focus();
            return false;
        }
        //认证编号为空就跳过判断
        if (ReferenceNumber == "") {
            return false;
        }
        for (var i = 0; i < num; i++) {
            var str = $("#Content").children().eq(index - 1).siblings().eq(i);
            var NameSelect = str.children().eq(0).find("select").val();
            var ReferenceNumberInput = str.children().eq(1).find("input").val();
            if (Name == NameSelect) {
                if (ReferenceNumber == ReferenceNumberInput) {
                    swal({
                        title: "操作失败",
                        text: "相同的认证名称，认证编号不能相同",
                        type: "error",
                        buttons: {
                            button1: {
                                text: "确认"
                            }
                        }
                    }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                        $("#ReferenceNumber" + index).focus();
                        $("#ReferenceNumber" + index).val("");
                    });
                    return false;
                }
            }
        }
        $.ajax({
            type: "post",
            dataType: 'JSON',
            url: "/CertificationApplication/CertificationApplicationSQL/CheckReferenceNumberAndSonName",
            data: { "Name": Name, "ReferenceNumber": ReferenceNumber },
            success: function (result) {
                if (result > 0) {
                    swal({
                        title: "操作失败",
                        text: "相同的认证名称，认证编号不能相同",
                        type: "error",
                        buttons: {
                            button1: {
                                text: "确认"
                            }
                        }
                    }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                        $("#ReferenceNumber" + index).focus();
                        $("#ReferenceNumber" + index).val("");
                    });
                }
            }
        });
    }
}


