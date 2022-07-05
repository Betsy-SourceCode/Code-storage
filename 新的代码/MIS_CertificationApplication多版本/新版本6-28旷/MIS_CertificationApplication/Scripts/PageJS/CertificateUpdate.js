var app = angular.module('myApp', []);  //创建模块
app.controller('mycontroller', function ($scope, $compile) {
    var ArrCovereArea = ""; //存放数据库的多选框 - 国家区域的值  如PL

    //认证管理数据查询
    $scope.CertificatesManagementList = function () {
        //国家区域
        //图片单选国家区域按钮将  地区区域的代码简称赋值给了 当前input标签的id
        var CountryArea = $("input[name='CountryArea']").attr("id");
        //k3代码
        var key_words = $("#key_words").val();
        $("#key").html(key_words);
        $("#country").html(CountryArea);
        var Cancel = true;
        if ($("#Cancel").is(":checked")) {
            Cancel = true;
        }
        else {
            Cancel = false;
        }
        $.ajax({
            url: "/CertificationApplication/CertificationApplicationSQL/GetCertificatesManagementList",
            type: 'post',
            dataType: 'json',
            data: { 'CountryAreas': CountryArea, 'key_words': key_words, "Cancel": Cancel },
            success: function (res) {
                console.log(res);
                if (res.length == 0) {
                    $scope.list = null;
                    $scope.$apply();
                    //显示无数据时 展示的tbody
                    $("#Message").html("未找到任何记录");
                    $("#NullList").css("display", "");
                    //禁用打印和导出按钮
                    $("#btnPrint").attr("disabled", true);
                    $("#btnDaochu").attr("disabled", true);
                }
                else {
                    //打开打印和导出按钮
                    $("#btnPrint").removeAttr("disabled");
                    /*$("#btnDaoChu").removeAttr("disabled");*/
                    $("#btnDaochu").removeAttr("disabled");
                    //隐藏显示无数据时 展示的tbody
                    $("#NullList").css("display", "none");
                    $scope.list = res;
                    $scope.$apply();
                }
            },
            error: function (res) {
                //debugger;
                //swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
            }
        });
    }

    //For Country/Area 输入框图标点击选择数据
    $scope.CountryArea = function () {
        ArrCovereArea = "";
        layer.open({
            type: 2,
            title: '挑 选 一 个 国 家 或 联 盟 区 域',
            skin: 'layui-layer-rim', //加上边框
            area: ['1250px', '80%'], //宽高
            content: '../CertificationApplication/SelectCertificatesCountriesAreas',
            btn: ['返回'],
            btnAlign: 'c',
            btn1: function (index, layero) { //提交并返回

                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为edit_category的元素，并获取其值赋值给g
                var arr = "";

                let g = body.find('input[name="test"]:checked').each(function () {
                    arr += $(this).val();
                    ArrCovereArea = this.id;  //存入数据库的  模糊查询地区代码
                })
                //if (ArrCovereArea.length > 0) {
                //    ArrCovereArea = ArrCovereArea.substring(0, ArrCovereArea.length - 1);
                //}
                //给自定义属性赋值
                $("input[name='CountryArea']").val(arr);
                $("input[name='CountryArea']").attr('id', ArrCovereArea);
                //关闭该子页面
                layer.close(index);
            },
            success: function (layero, index) {
                /* debugger*/
                //渲染按钮的样式
                $(".layui-layer-btn0").addClass("SelectcovereArea");
            }
        });
    }
    var SimpleArea;//地区简称
    //新增操作//
    $scope.AddCertificates = function (CertCode, CertName, country, StdFee, StdTime, Remark, Mkt_Cnty) {
        layer.open({
            type: 2,
            title: '新  增  认  证  证  书',
            skin: 'layui-layer-rim', //加上边框
            area: ['90%', '90%'], //宽高
            content: '../CertificationApplication/AddCertificates?CertCode=' + CertCode + '&CertName=' + CertName + '&CountryArea=' + country + '&StdFee=' + StdFee + '&StdTime=' + StdTime + '&Remark=' + Remark + '&Mkt_Cnty=' + Mkt_Cnty,
            btn: ['Confirm', 'Cancel'],
            btnAlign: 'c',
            btn1: function (index, layero) { //提交并返回
                //confirm
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为CertCodeInput的元素，并获取其值赋值给g
                let CertCode = body.find('#CertCodeInput').val();
                if (CertCode == "") {
                    body.find('#CertCodeTip').html("*");//2代表是comfirm按钮 不执行失焦事件
                    return false;
                } else {
                    body.find('#CertCodeTip').html("");
                }
                let CertName = body.find('#CertName').val();
                if (CertName == "") {
                    body.find('#CertNameTip').html("*");
                    body.find('#CertName').focus();
                    return false;
                } else {
                    body.find('#CertNameTip').html("");
                }

                //body.find('#country').attr('id',);
                body.find('input[name="CountryArea"]').attr('id', SimpleArea);
                let Mkt_Cnty = body.find('input[name="CountryArea"]').each(function () {
                    country = $(this).val();
                    ArrCovereArea = this.id;  //存入数据库的  模糊查询地区代码
                })

                if (country == "") {
                    body.find('#CountryAreaTip').html("*");
                    return false;
                } else {
                    body.find('#CountryAreaTip').html("");
                }
                SimpleArea = ArrCovereArea;
                let StdFee = body.find('#StandardFees').val();
                let StdTime = body.find('#StandardTime').val();
                let Remark = body.find('#Remarks').val();

                if (CertCode != "" || CertName != "" || ArrCovereArea != "" || country != "") {
                    //执行新增操作
                    $.ajax({
                        url: "/CertificationApplication/CertificationApplicationSQL/AddCertificatesManagementList",
                        type: 'post',
                        dataType: 'json',
                        data: { 'CertCode': CertCode, 'CertName': CertName, 'Mkt_Cnty': ArrCovereArea, 'StdFee': StdFee, 'StdTime': StdTime, 'Remark': Remark },
                        success: function (res) {
                            //CertificateMasterDetails界面
                            if (res > 0) {
                                $scope.CertificateMasterDetails(res);//res为CMSerial 数据库唯一标识
                                layer.close(index);
                            } else {
                                swal('Comfirm失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                            }
                        },
                        error: function (res) {
                            //debugger;
                            swal('Comfirm失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                        }
                    });
                }
                else {
                    swal('认证代码,认证名称,适用国家/区域!', '以上字段为必填项！', 'error')
                }

            },
            btn2: function (index, layero) {
                $scope.CertificatesManagementList();
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
    //认证详情界面弹窗  未作废的
    $scope.CertificateMasterDetails = function (CMSerial) {
        layer.open({
            type: 2,
            title: '认  证  证  书  明  细',
            skin: 'layui-layer-rim', //加上边框
            area: ['700px', '90%'], //宽高
            content: '../CertificationApplication/CertificateMasterDetails?CMSerial=' + CMSerial,
            btn: ['Copy', 'Edit', 'Void', 'Close'],
            btnAlign: 'c',
            btn1: function (index, layero) {
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为CertCodeInput的元素，并获取其值赋值给g
                let CertCode = body.find('#CertCode').text();
                let CertName = body.find('#CertName').text();
                body.find('div[name="CountryArea"]').attr('id', SimpleArea);
                let country = body.find('div[name="CountryArea"]').text();
                let Mkt_Cnty = body.find('label[name="Mkt_Cnty"]').attr("id");
                let StdFee = body.find('#StandardFees').text();
                let StdTime = body.find('#StandardTime').text();
                let Remark = body.find('#Remarks').text();
                $scope.AddCertificates(CertCode, CertName, country, StdFee, StdTime, Remark, Mkt_Cnty);
                layer.close(index);
            },
            btn2: function (index, layero) {
                //Edit
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为CertCodeInput的元素，并获取其值赋值给g
                let CertCode = body.find('#CertCode').text();
                $scope.CertificateEdit(CertCode, 0);
            },
            btn3: function (index, layero) {
                var body = layer.getChildFrame('body', index);
                let CMSerial = body.find('label[name="CMSerial"]').attr("id");
                //作废
                $scope.ISVoid(CMSerial);
                //Void
                layer.close(index);
            },
            btn4: function (index, layero) {
                //Close
                $scope.CertificatesManagementList();
                layer.close(index);
            },
            success: function (layero, index) {
                /* debugger*/
                //渲染按钮的样式
                $(".layui-layer-btn0").addClass("SelectCertificateMasterDetails");
                $(".layui-layer-btn1").addClass("SelectCertificateMasterDetails");
                $(".layui-layer-btn2").addClass("SelectCertificateMasterDetails");
                $(".layui-layer-btn3").addClass("SelectCertificateMasterDetails");
            }
        });
    }
    //认证详情编辑  作废的
    $scope.ModifyCertificateMasterDetails = function (CMSerial) {
        layer.open({
            type: 2,
            title: '认  证  证  书  明  细',
            skin: 'layui-layer-rim', //加上边框
            area: ['700px', '90%'], //宽高
            content: '../CertificationApplication/CertificateMasterDetails?CMSerial=' + CMSerial,
            btn: ['Copy', 'Save', 'Close'],
            btnAlign: 'c',
            btn1: function (index, layero) {
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为CertCodeInput的元素，并获取其值赋值给g
                let CertCode = body.find('#CertCode').text();
                let CertName = body.find('#CertName').text();
                body.find('input[name="CountryArea"]').attr('id', SimpleArea);
                let CountryArea = body.find('div[name="CountryArea"]').text();
                let Mkt_Cnty = body.find('label[name="Mkt_Cnty"]').attr("id");
                let StdFee = body.find('#StandardFees').text();
                let StdTime = body.find('#StandardTime').text();
                let Remark = body.find('#Remarks').text();
                $scope.AddCertificates(CertCode, CertName, CountryArea, StdFee, StdTime, Remark, Mkt_Cnty);
                layer.close(index);
            },
            btn2: function (index, layero) {
                //Save
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为CertCodeInput的元素，并获取其值赋值给g
                let CertCode = body.find('#CertCodeInput').val();
                let CertName = body.find('#CertName').val();
                //body.find('#country').attr('id',);
                body.find('input[name="CountryArea"]').attr('id', SimpleArea);
                let Mkt_Cnty = body.find('input[name="CountryArea"]').each(function () {
                    country = $(this).val();
                    ArrCovereArea = this.id;  //存入数据库的  模糊查询地区代码
                })
                SimpleArea = ArrCovereArea;
                let StdFee = body.find('#StandardFees').val();
                let StdTime = body.find('#StandardTime').val();
                let Remark = body.find('#Remarks').val();

                //执行新增操作
                $.ajax({
                    url: "/CertificationApplication/CertificationApplicationSQL/AddCertificatesManagementList",
                    type: 'post',
                    dataType: 'json',
                    data: { 'CertCode': CertCode, 'CertName': CertName, 'Mkt_Cnty': ArrCovereArea, 'StdFee': StdFee, 'StdTime': StdTime, 'Remark': Remark },
                    success: function (res) {
                        //CertificateMasterDetails界面
                        if (CertCode != "") {
                            $scope.CertificateMasterDetails(CertCode);
                            layer.close(index);
                        }
                    },
                    error: function (res) {
                        //debugger;
                        //swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                    }
                });

            },
            btn3: function (index, layero) {
                $scope.CertificatesManagementList();
                //Close
                layer.close(index);
            },
            success: function (layero, index) {
                /* debugger*/
                //渲染按钮的样式
                $(".layui-layer-btn0").addClass("SelectCertificateMasterDetails");
                $(".layui-layer-btn1").addClass("SelectCertificateMasterDetails");
                $(".layui-layer-btn2").addClass("SelectCertificateMasterDetails");
            }
        });
    }
    //作废的方法
    $scope.ISVoid = function (CMSerial) {
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
                $.ajax({
                    url: "/CertificationApplication/CertificationApplicationSQL/UpdateCertificatesISVoid?CMSerial=" + CMSerial,
                    type: 'post',
                    dataType: 'json',
                    success: function (res) {
                        if (res > 0) {
                            swal('作废成功！', '', 'success');
                            $scope.CertificatesManagementList();
                            $scope.$apply();
                        }

                    },
                    error: function (res) {
                        //debugger;
                        swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                    }
                });
            }
        })
    }
    //认证编辑Edit界面弹窗  未作废的以及作废的
    $scope.CertificateEdit = function (CertCode, IsVoid) {
        layer.open({
            type: 2,
            title: '维  护  认  证  证  书  信  息',
            skin: 'layui-layer-rim', //加上边框
            area: ['700px', '90%'], //宽高
            content: '../CertificationApplication/CertificateEdit?CertCode=' + CertCode + '&IsVoid=' + IsVoid,
            btn: ['Copy', 'Save', 'Close'],
            btnAlign: 'c',
            btn1: function (index, layero) {
                //copy
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为CertCodeInput的元素，并获取其值赋值给g
                var CertCode = body.find('#CertCode').text();
                let CertName = body.find('#CertName').val();
                //body.find('input[name="CountryArea"]').attr('id', SimpleArea);
                let country = body.find('input[name="CountryArea"]').val();
                let Mkt_Cnty = body.find('input[name="CountryArea"]').attr("id");
                let StdFee = body.find('#StandardFees').val();
                let StdTime = body.find('#StandardTime').val();
                let Remark = body.find('#Remarks').val();
                $scope.AddCertificates(CertCode, CertName, country, StdFee, StdTime, Remark, Mkt_Cnty);
                layer.close(index);
            },
            btn2: function (index, layero) {
                //save
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为CertCodeInput的元素，并获取其值赋值给g
                let CertCode = body.find('#CertCode').text();
                let CertName = body.find('#CertName').val();
                let Mkt_Cnty = body.find('input[name="CountryArea"]').attr('id');
                let country = body.find('input[name="CountryArea"]').val();
                let StdFee = body.find('#StandardFees').val();
                let StdTime = body.find('#StandardTime').val();
                let Remark = body.find('#Remarks').val();
                //Save
                $.ajax({
                    url: '/CertificationApplication/CertificationApplicationSQL/CertificateEdit?CertCode=' + CertCode + '&CertName=' + CertName + '&CountryArea=' + country + '&StdFee=' + StdFee + '&StdTime=' + StdTime + '&Remark=' + Remark + '&Mkt_Cnty=' + Mkt_Cnty,
                    type: 'post',
                    dataType: 'json',
                    success: function (res) {
                        if (res > 0) {
                            swal('保存成功！', '', 'success');
                            $scope.CertificatesManagementList();
                        }
                        $scope.CertificatesManagementList();//刷新列表
                        //Close
                        layer.close(index);
                    },
                    error: function (res) {
                        //debugger;
                        //swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                    }
                });
            },
            btn3: function (index, layero) {
                $scope.CertificatesManagementList();
                //Close
                layer.close(index);
            },
            success: function (layero, index) {
                /* debugger*/
                //渲染按钮的样式
                $(".layui-layer-btn0").addClass("SelectCertificateMasterDetails");
                $(".layui-layer-btn1").addClass("SelectCertificateMasterDetails");
                $(".layui-layer-btn2").addClass("SelectCertificateMasterDetails");
            }
        });
    }
})
//导出
function DaoChu() {
    var myDate = new Date();
    /*    var time = myDate.Format("yyyyMMddhhmm");  //获得当前年月日时分秒*/
    $("#DaoChuDiv").table2excel({
        // 不被导出的表格行的CSS class类
        exclude: ".noExl",
        // 导出的Excel文档的名称
        name: "Excel Document Name",
        // Excel文件的名称
        filename: "测试.xls"
        /* specialStyle: ["fontred", "changebgcolor"]*/
    });
}
//打印
function Dayin() {
    //将值传到打印界面
    var CountryArea = $("input[name='CountryArea']").attr("id");
    //k3代码
    var key_words = $("#key_words").val();
    $("#key").html(key_words);
    $("#country").html(CountryArea);
    var Cancel = true;
    if ($("#Cancel").is(":checked")) {
        Cancel = true;
    }
    else {
        Cancel = false;
    }
    document.getElementById("iframeId").contentWindow.Dayin(CountryArea, key_words, Cancel);  //contentWindow-指定的iframe或者iframe所在的Window对象
}

