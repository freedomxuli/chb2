var UserID = window.queryString.UserID;
var YunDanDenno = window.queryString.YunDanDenno;

var newcity = {};
var newqx = {};

var xuhao = 0;
var writechange = 1;//1:扫码；2：输入；
var fieldObj;

var detailStore = Ext.create('Ext.data.Store', {
    fields: [
        'GoodsName', 'GoodsPack', 'GoodsNum', 'GoodsWeight', 'GoodsVolume', 'ID'
    ]
});

Ext.onReady(function () {
    Ext.define('MainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            //align: 'center',
            type: 'column'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'panel',
                        height: document.documentElement.clientHeight,
                        columnWidth: 0.5,
                        autoScroll: true,
                        layout: {
                            type: 'column'
                        },
                        border: 0,
                        items: [
                            {
                                xtype: 'fieldset',
                                title: '必填区',
                                columnWidth: 1,
                                layout: {
                                    type: 'column'
                                },
                                id: 'btq',
                                items: [
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 20 10 20',
                                        id: 'GpsDeviceID',
                                        readOnly: true,
                                        fieldLabel: '设备码'
                                    },
                                    {
                                        xtype: 'container',
                                        columnWidth: 1,
                                        layout: 'column',
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 0.5,
                                                padding: '0 10 10 20',
                                                id: 'QiShiZhan_Province',
                                                readOnly: true,
                                                fieldLabel: '出发地'
                                            },
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 0.25,
                                                padding: '0 10 10 0',
                                                id: 'QiShiZhan_City',
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 0.25,
                                                padding: '0 10 10 0',
                                                id: 'QiShiZhan_Qx',
                                                readOnly: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 10 10 20',
                                        id: 'DaoDaZhan_Province',
                                        readOnly: true,
                                        fieldLabel: '目的地'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.25,
                                        padding: '0 10 10 0',
                                        id: 'DaoDaZhan_City',
                                        readOnly: true
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.25,
                                        padding: '0 10 10 0',
                                        id: 'DaoDaZhan_Qx',
                                        readOnly: true
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 10 10 20',
                                        id: 'SuoShuGongSi',
                                        readOnly: true,
                                        fieldLabel: '建单公司'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 0.5,
                                        padding: '0 10 10 10',
                                        id: 'UserDenno',
                                        readOnly: true,
                                        labelWidth: 50,
                                        fieldLabel: '建单号'
                                    },
                                    {
                                        xtype: 'textfield',
                                        columnWidth: 1,
                                        padding: '0 10 10 20',
                                        id: 'Expect_Hour',
                                        readOnly: true,
                                        minValue: 1,
                                        fieldLabel: '预计到达时间（小时）'
                                    },
                                    {
                                        xtype: 'container',
                                        columnWidth: 1,
                                        layout: 'column',
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                columnWidth: 1,
                                                padding: '0 10 10 20',
                                                id: 'MessageTel',
                                                readOnly: true,
                                                fieldLabel: '推送短信'
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                title: '选填区',
                                layout: {
                                    type: 'column'
                                },
                                columnWidth: 1,
                                id: 'xtq',
                                items: [
                                    
                                ]
                            }
                        ],
                        buttonAlign: 'center',
                        buttons: [
                            {
                                text: '返回',
                                iconCls: 'back',
                                handler: function () {
                                    FrameStack.popFrame();
                                }
                            }
                        ]
                    },
                    {
                        xtype: 'gridpanel',
                        columnLines: 1,
                        border: 1,
                        height: document.documentElement.clientHeight,
                        columnWidth: 0.5,
                        autoScroll: true,
                        store: detailStore,
                        id: 'detailStore',
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GoodsName',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '货物名称'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GoodsPack',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '包装'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GoodsNum',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '数量'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GoodsWeight',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '重量'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GoodsVolume',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '体积'
                            }
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }

    });

    new MainView();

    dataBind();

});

function dataBind() {
    var btq_arr = [];
    var xtq_arr = [];
    CS('CZCLZ.Handler.GetSelectionModelByUser', function (retVal) {
        if (retVal) {
            fieldObj = retVal;
            var ishasxt = false;
            for (var i = 0; i < retVal.length; i++) {
                if (retVal[i]["DingDanSetListLX"] == "1") {
                    ishasxt = true;
                    if (retVal[i]["DingDanSetListBS"]) {
                        var div = Ext.create("Ext.form.field.Text", {
                            columnWidth: 1,
                            padding: '0 20 10 20',
                            readOnly: true,
                            id: retVal[i]["DingDanSetListBS"],
                            fieldLabel: retVal[i]["DingDanSetListMC"]
                        });
                        xtq_arr.push(div);
                    }
                    else {
                        var div = Ext.create("Ext.form.field.Text", {
                            columnWidth: 1,
                            padding: '0 20 10 20',
                            readOnly: true,
                            id: "div" + retVal[i]["DingDanSetListPX"],
                            fieldLabel: retVal[i]["DingDanSetListMC"]
                        });
                        xtq_arr.push(div);
                    }
                } else {
                    if (retVal[i]["DingDanSetListBS"]) {
                        var div = Ext.create("Ext.form.field.Text", {
                            columnWidth: 1,
                            padding: '0 10 10 20',
                            id: retVal[i]["DingDanSetListBS"],
                            readOnly: true,
                            fieldLabel: retVal[i]["DingDanSetListMC"]
                        });
                        btq_arr.push(div);
                    }
                    else {
                        var div = Ext.create("Ext.form.field.Text", {
                            columnWidth: 1,
                            padding: '0 10 10 20',
                            id: "div" + retVal[i]["DingDanSetListPX"],
                            readOnly: true,
                            fieldLabel: retVal[i]["DingDanSetListMC"]
                        });
                        btq_arr.push(div);
                    }
                }
            }
            if (!ishasxt)
                Ext.getCmp("xtq").hide();
            Ext.getCmp("btq").add(btq_arr);
            Ext.getCmp("xtq").add(xtq_arr);

            CS('CZCLZ.Handler.EditYunDanNew', function (retVal) {
                if (retVal) {
                    Ext.getCmp("GpsDeviceID").setValue(retVal.dt[0]["GpsDeviceID"]);
                    Ext.getCmp("QiShiZhan_Province").setValue(retVal.dt[0]["QiShiZhan"].split(' ')[0]);
                    Ext.getCmp("QiShiZhan_City").setValue(retVal.dt[0]["QiShiZhan"].split(' ')[1]);
                    Ext.getCmp("QiShiZhan_Qx").setValue(retVal.dt[0]["QiShiZhan_QX"]);
                    Ext.getCmp("DaoDaZhan_Province").setValue(retVal.dt[0]["DaoDaZhan"].split(' ')[0]);
                    Ext.getCmp("DaoDaZhan_City").setValue(retVal.dt[0]["DaoDaZhan"].split(' ')[1]);
                    Ext.getCmp("DaoDaZhan_Qx").setValue(retVal.dt[0]["DaoDaZhan_QX"]);
                    Ext.getCmp("SuoShuGongSi").setValue(retVal.dt[0]["SuoShuGongSi"]);
                    Ext.getCmp("UserDenno").setValue(retVal.dt[0]["UserDenno"]);
                    Ext.getCmp("Expect_Hour").setValue(retVal.dt[0]["Expect_Hour"]);
                    Ext.getCmp("MessageTel").setValue(retVal.dt[0]["MessageTel"]);

                    for (var i = 0; i < retVal.dt_field.length; i++)
                    {
                        Ext.getCmp(retVal.dt_field[i]["DingDanSetListBS"]).setValue(retVal.dt[0][retVal.dt_field[i]["DingDanSetListBS"]]);
                    }

                    for (var i = 0; i < retVal.dt_field_valule.length; i++) {
                        Ext.getCmp(retVal.dt_field_valule[i]["YunDanFieldBS"]).setValue(retVal.dt_field_valule[i]["YunDanFieldValue"]);
                    }
                    detailStore.loadData(retVal.dt_detail);
                }
            }, CS.onError, YunDanDenno);
        }
    }, CS.onError);
}