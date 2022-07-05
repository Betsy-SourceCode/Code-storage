var app = angular.module('myApp', []);  //创建模块
//K3相关物料 的失焦事件
function BlurK3Parts(valueStr, thisInput) {
    var _that = thisInput;
    if (valueStr != "") {//客户在填写"K3 相关物料号" 后
        //检查是否存在于GIPComponent 表  不存在提示 所填的物料号不存在于公司的物料总表里，请查核及修正
        $.ajax({
            url: "/CertificationApplication/CertificationApplicationSQL/GetK3PartsISExsits",
            type: 'post',
            dataType: 'json',
            data: { 'Fnumber': valueStr },
            success: function (res) {
                //console.log(res);
                if (res.length == 0) {
                    swal({
                        title: "所填的物料号不存在于公司的物料总表里，请查核及修正",
                        text: "",
                        icon: "error",
                        buttons: {
                            button1: {
                                text: "确认",
                                value: true,
                            }
                        }
                    }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                        $(_that).focus();
                        $(_that).val("");
                    });
                }
                return false;
            },
            error: function (res) {
                //debugger;
                //swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
            }
        });
    }
}
app.controller('mycontroller', function ($scope, $compile) {
    //产品模型管理数据查询
    $scope.ComponentModelManagement = function () {
        $("#XMModal").modal({ backdrop: 'static', keyboard: false }); //打开熊猫加载图片
        //k3代码
        var K3ItemsNum = $("#K3ItemsNum").val();
        //key_words
        var key_words = $("#key_words").val();
        $("#key").html(key_words);//给表格上方的条件详情显示区域  赋值
        $("#k3").html(K3ItemsNum);
        $.ajax({
            url: "/CertificationApplication/CertificationApplicationSQL/GetComponentModelManagementList",
            type: 'post',
            dataType: 'json',
            data: { 'K3ItemsNum': K3ItemsNum, 'key_words': key_words,'flag':0 },//flag=0时  是查询
            success: function (res) {
                //console.log(res);
                if (res.length == 0) {
                    $scope.list = null;
                    $scope.$apply();
                    //显示无数据时 展示的tbody
                    $("#Message").html("未找到任何记录");
                    $("#NullList").css("display", "");
                    //禁用打印和导出按钮
                    $("#btnPrint").attr("disabled", true);
                    $("#btnDaochu").attr("disabled", true);
                    //加载完成后  关闭熊猫遮罩层
                    $("#XMModal").modal('hide');
                }
                else {
                    //打开打印和导出按钮
                    $("#btnPrint").removeAttr("disabled");
                    //$("#btnDaochu").removeAttr("disabled");
                    //隐藏显示无数据时 展示的tbody
                    $("#NullList").css("display", "none");
                    $scope.list = res;
                    $scope.$apply();
                    //加载完成后  关闭熊猫遮罩层
                    $("#XMModal").modal('hide');
                }
            },
            error: function (res) {
                //debugger;
                //swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
            }
        });
    }
    //新增 模型产品的方法
    //modecode   模型代码
    //modelName  模型名称
    //ModelSpec  模型描述
    //arr        k3代码
    $scope.AddProductModel = function (ModelCode, ModelName, ModelSpec, arr) {
        //新增的ajax
        $.ajax({
            url: "/CertificationApplication/CertificationApplicationSQL/AddComponentModelList?ModelCode=" + ModelCode + "&ModelName=" + ModelName + "&ModelSpec=" + ModelSpec + "&K3Parts=" + arr,
            type: 'post',
            dataType: 'json',
            success: function (res) {
                if (res > 0) {
                    swal('新增成功！', '', 'success');
                    $scope.ComponentModelManagement();
                    $scope.$apply();
                }

            },
            error: function (res) {
                //debugger;
                swal('新增元器件模型失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
            }
        });
    }
    //产品模型新增界面 Add  1
    $scope.AddComponentModel = function (CPSerial) {
        layer.open({
            type: 2,
            title: '新  增  元  器  件  型  号',
            skin: 'layui-layer-rim', //加上边框
            area: ['1250px', '80%'], //宽高
            content: '../CertificationApplication/AddCopyEditProduct_Model?CPSerial=' + CPSerial + '&type=1',
            btn: ['Confirm', 'Cancel'],
            btnAlign: 'c',
            btn1: function (index, layero) { //新增模型
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为edit_category的元素，并获取其值赋值给g
                var ModelCode = body.find("#ModelCode").val();
                if (ModelCode == "") {
                    body.find("#ModelCodeTip").html("*");
                    body.find("#ModelCode").focus();
                    return false;
                } else {
                    body.find("#ModelCodeTip").html("");
                }
                var ModelName = body.find("#ModelName").val();
                if (ModelName == "") {
                    body.find("#ModelNameTip").html("*");
                    body.find("#ModelName").focus();
                    return false;
                } else {
                    body.find("#ModelNameTip").html("");
                }
                var ModelSpec = body.find("#ModelSpec").val();
                var arr = "";//所有输入的k3物料号 arr
                var result1;//用于判断物料号是否存在公司物料表中
                var result2;//用于判断物料号是否存在公司物料表中
                var K3Parts = body.find("div[class='k3Div']").each(function () {//在页面中找div下的input标签
                    var _that = this;//代表当前input
                    var num = 0;//代表  input   循环的次数
                    var fnumber1 = "";//物料号1
                    var fnumber2 = "";//物料号2
                    $(_that).find("input").each(function () {//循环每一个input
                        if ($(this).val() != "") {//判断input是否有值
                            body.find("#K3PartsTip").html("");
                            if (num == 0) {//第一次进入的时候
                                fnumber1 = $(this).val();//存值
                                num += 1;//num加一  证明已经循环到了第几个input
                            } else {
                                fnumber2 = $(this).val();//num>0代表已经是第二个input的值了
                                num = 0;//再将num的值还原为初始值0
                            }
                            //检查是否存在公司物料表中
                            if (fnumber1 != "") {
                                //检查是否存在于GIPComponent 表  不存在提示 所填的物料号不存在于公司的物料总表里，请查核及修正
                                $.ajax({
                                    async: false,
                                    url: "/CertificationApplication/CertificationApplicationSQL/GetK3PartsISExsits",
                                    type: 'post',
                                    dataType: 'json',
                                    data: { 'Fnumber': fnumber1 },
                                    success: function (res) {
                                        //console.log(res);
                                        if (res.length == 0) {
                                            swal({
                                                title: "所填的物料号不存在于公司的物料总表里，请查核及修正",
                                                text: "",
                                                icon: "error",
                                                buttons: {
                                                    button1: {
                                                        text: "确认",
                                                        value: true,
                                                    }
                                                }
                                            }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                                                $(this).focus();
                                                $(this).val("");
                                            });
                                            result1 = false;
                                        } else {
                                            result1 = true;
                                        }

                                    },
                                    error: function (res) {
                                        //debugger;
                                        //swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                                    }
                                });
                            }
                            if (fnumber2 != "") {
                                //检查是否存在于GIPComponent 表  不存在提示 所填的物料号不存在于公司的物料总表里，请查核及修正
                                $.ajax({
                                    async: false,
                                    url: "/CertificationApplication/CertificationApplicationSQL/GetK3PartsISExsits",
                                    type: 'post',
                                    dataType: 'json',
                                    data: { 'Fnumber': fnumber2 },
                                    success: function (res) {
                                        //console.log(res);
                                        if (res.length == 0) {
                                            swal({
                                                title: "所填的物料号不存在于公司的物料总表里，请查核及修正",
                                                text: "",
                                                icon: "error",
                                                buttons: {
                                                    button1: {
                                                        text: "确认",
                                                        value: true,
                                                    }
                                                }
                                            }).then(function (value) {   //这里的value就是按钮的value值，只要对应就可以啦
                                                $(this).focus();
                                                $(this).val("");
                                            });
                                            result2 = false;
                                        } else {
                                            result2 = true;
                                        }

                                    },
                                    error: function (res) {
                                        //debugger;
                                        //swal('保存失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                                    }
                                });
                            }
                        }
                    });


                    //两个物料号比较大小
                    $.ajax({
                        async: false,
                        url: "/CertificationApplication/CertificationApplicationSQL/CompareK3Parts?fnumber1=" + fnumber1 + "&fnumber2=" + fnumber2,
                        type: 'post',
                        dataType: 'json',
                        success: function (res) {
                            if (res != "") {

                                arr += res + ";"
                            }
                        },
                        error: function (res) {
                            //debugger;
                            swal('新增物料号失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                        }
                    });

                })
                //物料号的集合 arr
                arr = arr.substring(0, arr.lastIndexOf(';'));
                if (arr == "") {
                    $(this).focus();
                    body.find(".K3PartsTip").html("*");
                    return false;
                }
                //result 为false时，就是k3在
                if (result1 == false || result2 == false) {
                    return false;
                }
                if (arr.length <= 500) {
                    //调用新增模型产品的方法
                    $scope.AddProductModel(ModelCode, ModelName, ModelSpec, arr);
                    layer.close(index);
                } else {
                    layer.close(index);
                    swal('【K3 相关物料号】新增的数据长度过长!', '请重新输入长度不超过500的【K3 相关物料号】', 'error')
                }
            },
            btn2: function (index, layero) {
                layer.close(index);
            },
            success: function (layero, index) {
                /* debugger*/
                $(".layui-layer-btn0").addClass("SelectCertificateMasterDetails");
                $(".layui-layer-btn1").addClass("SelectCertificateMasterDetails");
            }
        });
    }
    //产品模型复制界面 Copy 2
    $scope.CopyComponentModel = function (CPSerial) {
        layer.open({
            type: 2,
            title: '复  制  至  新  元  器  件  型  号',
            skin: 'layui-layer-rim', //加上边框
            area: ['1250px', '80%'], //宽高
            content: '../CertificationApplication/AddCopyEditProduct_Model?CPSerial=' + CPSerial + '&type=2',
            btn: ['Confirm', 'Cancel'],
            btnAlign: 'c',
            btn1: function (index, layero) { //新增模型
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为edit_category的元素，并获取其值赋值给g
                // 得到找到body元素中id为edit_category的元素，并获取其值赋值给g
                var ModelCode = body.find("#ModelCode").val();
                if (ModelCode == "") {
                    body.find("#ModelCodeTip").html("*");
                    body.find("#ModelCode").focus();
                    return false;
                } else {
                    body.find("#ModelCodeTip").html("");
                }
                var ModelName = body.find("#ModelName").val();
                if (ModelName == "") {
                    body.find("#ModelNameTip").html("*");
                    body.find("#ModelName").focus();
                    return false;
                } else {
                    body.find("#ModelNameTip").html("");
                }
                var ModelSpec = body.find("#ModelSpec").val();
                var arr = "";//所有输入的k3物料号 arr
                var K3Parts = body.find("div[class='k3Div']").each(function () {//在页面中找div下的input标签
                    var _that = this;//代表当前input
                    var num = 0;//代表  input   循环的次数
                    var fnumber1 = "";//物料号1
                    var fnumber2 = "";//物料号2
                    $(_that).find("input").each(function () {//循环每一个input

                        if ($(this).val() != "") {//判断input是否有值
                            if (num == 0) {//第一次进入的时候
                                fnumber1 = $(this).val();//存值
                                num += 1;//num加一  证明已经循环到了第几个input
                            } else {
                                fnumber2 = $(this).val();//num>0代表已经是第二个input的值了
                                num = 0;//再将num的值还原为初始值0
                            }
                        }

                    });
                    //两个物料号比较大小
                    $.ajax({
                        async: false,
                        url: "/CertificationApplication/CertificationApplicationSQL/CompareK3Parts?fnumber1=" + fnumber1 + "&fnumber2=" + fnumber2,
                        type: 'post',
                        dataType: 'json',
                        success: function (res) {
                            if (res != "") {
                                arr += res + ";"
                            }
                        },
                        error: function (res) {
                            //debugger;
                            swal('新增物料号失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                        }
                    });
                })
                arr = arr.substring(0, arr.lastIndexOf(';'));//k3物料号
                if (arr == "") {
                    $(this).focus();
                    body.find(".K3PartsTip").html("*");
                    return false;
                }
                if (arr.length <= 500) {
                    //调用新增模型产品的方法
                    $scope.AddProductModel(ModelCode, ModelName, ModelSpec, arr);
                    layer.close(index);
                } else {
                    layer.close(index);
                    swal('【K3 相关物料号】新增的数据长度过长!', '请重新输入长度不超过500的【K3 相关物料号】', 'error')
                }
                layer.close(index);
            },
            btn2: function (index, layero) {
                layer.close(index);
            },
            success: function (layero, index) {
                /* debugger*/
                $(".layui-layer-btn0").addClass("SelectCertificateMasterDetails");
                $(".layui-layer-btn1").addClass("SelectCertificateMasterDetails");
            }
        });
    }
    //产品模型编辑 Edit 3
    $scope.EditComponentModel = function (CPSerial) {
        layer.open({
            type: 2,
            title: '修  改  元  器  件  型  号',
            skin: 'layui-layer-rim', //加上边框
            area: ['1250px', '80%'], //宽高
            content: '../CertificationApplication/AddCopyEditProduct_Model?CPSerial=' + CPSerial + '&type=3',
            btn: ['Save', 'Cancel'],
            btnAlign: 'c',
            btn1: function (index, layero) { //新增模型
                var _this;//代表input标签
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中id为CPSerial的元素，并获取其值赋值给g
                var CPSerial = body.find("div[name='CPSerial']").attr("id");
                var ModelCode = body.find("#ModelCode").text();
                var ModelName = body.find("#ModelName").val();
                if (ModelName == "") {
                    body.find("#ModelNameTip").html("*");
                    body.find("#ModelName").focus();
                    return false;
                } else {
                    body.find("#ModelNameTip").html("");
                }
                var ModelSpec = body.find("#ModelSpec").val();
                var arr = "";//所有输入的k3物料号 arr
                var K3Parts = body.find("div[class='k3Div']").each(function () {//在页面中找div下的input标签
                    var _that = this;//代表当前input
                    var num = 0;//代表  input   循环的次数
                    var fnumber1 = "";//物料号1
                    var fnumber2 = "";//物料号2
                    $(_that).find("input").each(function () {//循环每一个input
                        if ($(this).val() != "") {//判断input是否有值
                            if (num == 0) {//第一次进入的时候
                                _this = this;
                                fnumber1 = $(this).val();//存值
                                num += 1;//num加一  证明已经循环到了第几个input
                            } else {
                                fnumber2 = $(this).val();//num>0代表已经是第二个input的值了
                                num = 0;//再将num的值还原为初始值0
                            }
                        }

                    });
                    //两个物料号比较大小
                    $.ajax({
                        async: false,
                        url: "/CertificationApplication/CertificationApplicationSQL/CompareK3Parts?fnumber1=" + fnumber1 + "&fnumber2=" + fnumber2,
                        type: 'post',
                        dataType: 'json',
                        success: function (res) {
                            if (res != "") {
                                arr += res + ";"
                            }
                        },
                        error: function (res) {
                            //debugger;
                            swal('新增物料号失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                        }
                    });
                })
                arr = arr.substring(0, arr.lastIndexOf(';'));
                if (arr == "") {
                    $(_this).focus();
                    body.find(".K3PartsTip").html("*");
                    return false;
                }
                if (arr.length <= 500) {
                    //修改方法 UpdateProductModel
                    $.ajax({
                        url: "/CertificationApplication/CertificationApplicationSQL/UpdateProductModel?CPSerial=" + CPSerial + '&ModelCode=' + ModelCode + '&ModelName=' + ModelName + '&ModelSpec=' + ModelSpec + '&K3Parts=' + arr,
                        type: 'post',
                        dataType: 'json',
                        success: function (res) {
                            if (res > 0) {
                                swal('修改成功！', '', 'success');
                                $scope.ComponentModelManagement();
                                $scope.$apply();
                            }

                        },
                        error: function (res) {
                            //debugger;
                            swal('修改元器件模型失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                        }
                    });
                    layer.close(index);
                } else {
                    layer.close(index);
                    swal('【K3 相关物料号】新增的数据长度过长!', '请重新输入长度不超过500的【K3 相关物料号】', 'error')
                }


            },
            btn2: function (index, layero) {
                //关闭
                layer.close(index);
            },
            success: function (layero, index) {
                /* debugger*/
                //渲染按钮的样式
                $(".layui-layer-btn0").addClass("SelectCertificateMasterDetails");
                $(".layui-layer-btn1").addClass("SelectCertificateMasterDetails");
            }
        });
    }
    //产品模型详情  Detail
    $scope.ComponentModelDetail = function (CPSerial) {
        layer.open({
            type: 2,
            title: '元  器  件  型  号  详  情 ',
            skin: 'layui-layer-rim', //加上边框
            area: ['1250px', '80%'], //宽高
            content: '../CertificationApplication/Component_Model_Details?CPSerial=' + CPSerial,
            btn: ['Edit', 'Copy', 'Close'],
            btnAlign: 'c',
            btn1: function (index, layero) { //新增模型
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中name为CPSerial的元素，并获取其值赋值给g
                var CPSerial = body.find("div[name='CPSerial']").attr("id");
                $scope.EditComponentModel(CPSerial);
                layer.close(index);
            },
            btn2: function (index, layero) {
                //copy
                //获取子页面（iframe页）的body元素
                var body = layer.getChildFrame('body', index);
                // 得到找到body元素中name为CPSerial的元素，并获取其值赋值给g
                var CPSerial = body.find("div[name='CPSerial']").attr("id");
                $scope.CopyComponentModel(CPSerial);
                layer.close(index);
            },
            btn3: function (index, layero) {
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
    //产品模型  删除
    $scope.ISDel = function (CPSerial) {
        swal({
            title: '您确定删除此条数据?',
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
                    url: "/CertificationApplication/CertificationApplicationSQL/ComponentModelISDel?CPSerial=" + CPSerial,
                    type: 'post',
                    dataType: 'json',
                    success: function (res) {
                        if (res > 0) {
                            swal('删除成功！', '', 'success');
                            $scope.ComponentModelManagement();
                            $scope.$apply();
                        }

                    },
                    error: function (res) {
                        //debugger;
                        swal('删除失败!', '发生错误，请联系电脑部！内部成员请查看日志文件', 'error') //提示框
                    }
                });
            }
        })
    }
    //产品模型新增界面中的  新增一行input标签
    $scope.AddK3Input = function () {
        var Count = $("div[class='k3Div']").length;
        var div = '<div class="k3Div"  id="' + (Count + 1) + '"><input value = "" type = "text" id = "" name = "K3Parts" class="form-control k3Input" onblur = "BlurK3Parts(this.value,this.id)" /> <label>—</label> <input value="" type="text" id="" name="K3Parts" class="form-control k3Input" onblur="BlurK3Parts(this.value,this.id)"/><label class="K3PartsTip" style="color:red;font-size:20px"></label></div > ';
        $("#K3DivContainer").append(div);
    }
    //对比物料号大小的方法
    $scope.CompareK3Parts = function () {

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
    //k3代码
    var K3ItemsNum = $("#K3ItemsNum").val();
    var key_words = $("#key_words").val();
    document.getElementById("iframeId").contentWindow.Dayin(K3ItemsNum, key_words);  //contentWindow-指定的iframe或者iframe所在的Window对象
}
