
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
                        columns: [
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'BangDingTime',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
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
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '当前位置'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'YunDanDenno',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                hidden: true,
                                text: '随机运单号'
                            },
                            {
                                xtype: 'gridcolumn',
                                dataIndex: 'UserID',
                                flex: 1,
                                sortable: false,
                                menuDisabled: true,
                                text: '查看轨迹',
                                renderer: function (value, cellmeta, record, rowIndex, columnIndex, store) {
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
                                        id: 'SuoShuGongSi',
                                        fieldLabel: '公司'
                                    },
                                    {
                                        xtype: 'textfield',
                                        width: 160,
                                        labelWidth: 60,
                                        id: 'UserDenno',
                                        fieldLabel: '单号'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'search',
                                        text: '查询'
                                    },
                                    {
                                        xtype: 'button',
                                        iconCls: 'add',
                                        text: '查看轨迹',
                                        handler: function () {
                                            FrameStack.pushFrame({
                                                url: "chadanyundanguiji.html",
                                                onClose: function (ret) {
                                                    DataBind();
                                                }
                                            });
                                        }
                                    }
                                ]
                            },
                            {
                                xtype: 'pagingtoolbar',
                                dock: 'bottom',
                                width: 360,
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

    DataBind();
});

function DataBind() {

}

function ShowGJ(UserID, YunDanDenno) {
    FrameStack.pushFrame({
        url: "chadanyundanguiji.html?UserID=" + UserID + "&YunDanDenno=" + YunDanDenno + "&type=" + wodeyundan,
        onClose: function (ret) {
            DataBind();
        }
    });
}
