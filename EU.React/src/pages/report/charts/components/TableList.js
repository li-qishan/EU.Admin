import React, { Component } from 'react';
import { connect, FormattedMessage, formatMessage } from 'umi';
// import { query, BatchDelete, Delete } from '../service';
import { Input, Card, Form, Row, Col, Modal, message, Tabs } from 'antd';
import styles from '../../../dashboard/analysis/style.less';
const { TabPane } = Tabs;
import { TimelineChart, Pie } from '../../../dashboard/analysis/components/Charts';
import DataSet from "@antv/data-set";
import {
  G2, Chart, Geom, Axis, Tooltip, Coord, Label, Legend, View, Guide, Shape, Facet, Util
} from "bizcharts";

let moduleCode = "RM_FEE_ORDER_MNG";
let me;
let modelName = 'rmorder';
const FormItem = Form.Item;
const CustomTab = ({ data, currentTabKey: currentKey }) => (
  <Row
    gutter={8}
    style={{
      width: 138,
      margin: '8px 0',
    }}
    type="flex"
  >1
  </Row>
);
class TableList extends Component {
  formRef = React.createRef();
  formRef1 = React.createRef();
  actionRef = React.createRef();
  constructor(props) {
    super(props);
    me = this;
    me.state = {
      isModalVisible: false,
      Id: ''
    };
  }
  componentWillMount() {
    const { dispatch } = this.props;
    // dispatch({
    //     type: modelName + '/getModuleInfo',
    //     payload: { moduleCode },
    // })
  }
  handleTabChange = key => {
    this.setState({
      activeKey: key,
    });
  };
  render() {
    let { dispatch, rmorder: { moduleInfo, tableParam } } = this.props;
    let { activeKey } = this.state;
    const data = [
      {
        month: "2022/12",
        city: "染助劑",
        temperature: 343
      },
      {
        month: "2022/12",
        city: "包装材料",
        temperature: 34
      },
      {
        month: "2022/12",
        city: "修配费用",
        temperature: 128
      },
      {
        month: "2023/01",
        city: "染助劑",
        temperature: 256
      },
      {
        month: "2023/01",
        city: "包装材料",
        temperature: 78
      },
      {
        month: "2023/01",
        city: "修配费用",
        temperature: 115
      },
      {
        month: "2023/02",
        city: "染助劑",
        temperature: 259
      },
      {
        month: "2023/02",
        city: "包装材料",
        temperature: 113
      },
      {
        month: "2023/02",
        city: "修配费用",
        temperature: 101
      },
      {
        month: "2023/03",
        city: "染助劑",
        temperature: 223
      },
      {
        month: "2023/03",
        city: "包装材料",
        temperature: 99
      },
      {
        month: "2023/03",
        city: "修配费用",
        temperature: 121
      }
    ];
    const cols = {
      month: {
        range: [0, 1]
      }
    };

    const data1 = [
      {
        State: "2022/12",
        染助劑: 25635,
        "包装材料": 1890,
        "修配费用": 9314
      },
      {
        State: "2023/01",
        染助劑: 30352,
        "包装材料": 20439,
        "修配费用": 10225
      },
      {
        State: "2023/02",
        染助劑: 38253,
        "包装材料": 42538,
        "修配费用": 15757
      },
      {
        State: "2023/03",
        染助劑: 51896,
        "包装材料": 67358,
        "修配费用": 18794
      },
      {
        State: "2023/04",
        染助劑: 72083,
        "包装材料": 85640,
        "修配费用": 22153
      }
    ];
    data1.map(d => {
      const total = ["染助劑", "包装材料", "修配费用"].reduce((pre, f) => {
        pre += d[f];
        return pre;
      }, 0);
      d.Total = total;
    })

    const ds = new DataSet();
    const dv = ds.createView().source(data1);


    dv.transform({
      type: "fold",
      fields: ["染助劑", "包装材料", "修配费用"],
      // 展开字段集
      key: "年龄段",
      // key字段
      value: "人口数量",
      // value字段
      retains: ["State", 'Total'] // 保留字段集，默认为除fields以外的所有字段
    });

    return (
      <>
        <Card
          // loading={loading}
          className={styles.offlineCard}
          bordered={false}
          style={{
            marginTop: 32,
          }}
        >
          <Chart height={400} data={data} scale={cols} forceFit>
            <Legend />
            <Axis name="month" />
            <Axis
              name="temperature"
              label={{
                // formatter: val => `${val}°C`
                formatter: val => `${val}`
              }}
            />
            <Tooltip
              crosshairs={{
                type: "y"
              }}
            />
            <Geom
              type="line"
              position="month*temperature"
              size={2}
              color={"city"}
              shape={"smooth"}
            />
            <Geom
              type="point"
              position="month*temperature"
              size={4}
              shape={"circle"}
              color={"city"}
              style={{
                stroke: "#fff",
                lineWidth: 1
              }}
            />
          </Chart>
          <Chart height={400} data={dv} forceFit>
            <Legend />
            <Coord />
            <Axis
              name="State"
              label={{
                offset: 12
              }}
            />
            <Axis name="人口数量" />
            <Tooltip />
            <Geom
              type="intervalStack"
              position="State*人口数量"
              color={"年龄段"}
            >
              <Label content={['Total*年龄段', (t, n) => {
                if (n === '染助劑') {
                  return t;
                }
              }]} />
            </Geom>
          </Chart>
        </Card>
      </>
    );
  }
}
export default connect(({ rmorder }) => ({
  rmorder
}))(TableList);
