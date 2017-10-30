Ext.QuickTips.init();

var pageSize = 15;

var myStore = createSFW4Store({
    pageSize: pageSize,
    total: 1,
    currentPage: 1,
    fields: [
        'BangDingTime', 'UserDenno', 'QiShiZhan', 'DaoDaZhan', 'SuoShuGongSi', 'GpsDeviceID', 'YunDanRemark', 'Gps_lastinfo', 'YunDanDenno', 'UserID', 'Gps_lasttime'
    ],
    onPageChange: function (sto, nPage, sorters) {
        DataBind(nPage);
    }
});

Ext.onReady(function () {

    Ext.define('mainView', {
        extend: 'Ext.container.Viewport',

        layout: {
            type: 'fit'
        },

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                items: [
                    {
                        xtype: 'gridpanel',
                        columnLines: 1,
                        border: 1,
                        store: myStore,
                        columns: [
                            {
                                xtype: 'datecolumn',
                                dataIndex: 'BangDingTime',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                format: 'Y-m-d',
                                text: '日期'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UserDenno',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '运单号'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'QiShiZhan',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '起始站'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'DaoDaZhan',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '到达站'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'SuoShuGongSi',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '公司名称'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'GpsDeviceID',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '设备ID'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'YunDanRemark',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '运单备注'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'Gps_lastinfo',
                                flex: 3,
                                sortable: false,
                                menuDisabled: true,
                                text: '当前位置',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
                                    if (record.data.Gps_lasttime != "" && record.data.Gps_lasttime != null) {
                                        return "<span data-qtip='" + value + "'>" + "(" + record.data.Gps_lasttime.toCHString() + ")" + value + "</span>";
                                    }
                                    else {
                                        return "<span data-qtip='" + value + "'>" + value + "</span>";
                                        return value;
                                    }
                                }
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'YunDanDenno',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                hidden:true,
                                text: '随机运单号'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UserID',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '查看轨迹',
                                renderer: function (value,cellmeta,record,rowIndex,columnIndex,store) {
                                    return "<a href='javascript:void(0);' onClick='ShowGJ(\"" + value + "\",\"" + record.data.YunDanDenno + "\");'>查看轨迹</a>";
                                }
                            }
                        ],
                        dockedItems: [
                            {
                                xtype: 'toolbar',
                                dock: 'top',
                                items: [
                                    {
                                        xtype: 'textfield',
                                        width: 160,
                                        labelWidth: 60,
                                        id: 'UserDenno',
                                        fieldLabel: '单　　号'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'search',
                                        text: '查询',
                                        handler: function () {
                                            DataBind(1);
                                        }
                                    }
                                ]
                            },
                            {
                                xtype: 'pagingtoolbar',
                                dock: 'bottom',
                                width: 360,
                                store: myStore,
                                displayInfo: true
                            }
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }

    });

    new mainView();

    DataBind(1);
});

function DataBind(cp) {
    CS('CZCLZ.Handler.SearchMyYunDan', function (retVal) {
        if (retVal) {
            myStore.setData({
                data: retVal.dt,
                pageSize: pageSize,
                total: retVal.ac,
                currentPage: retVal.cp
            });
        }
    }, CS.onError, cp, pageSize, Ext.getCmp('UserDenno').getValue());
}

function ShowGJ(UserID, YunDanDenno) {
    FrameStack.pushFrame({
        url: "chadanyundanguiji.html?UserID=" + UserID + "&YunDanDenno=" + YunDanDenno + "&type=wodeyundan",
        onClose: function (ret) {
            
        }
    });
}
