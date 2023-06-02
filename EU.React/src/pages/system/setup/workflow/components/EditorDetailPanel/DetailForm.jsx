import React from 'react';
import { Card, Input, Select, Form } from 'antd';
import { withPropsAPI } from 'gg-editor';
import ComBoBox from '@/components/SysComponents/ComboBox';
import ComboGrid from '@/components/SysComponents/ComboGrid'

const upperFirst = str => str.toLowerCase().replace(/( |^)[a-z]/g, l => l.toUpperCase());

const { Item } = Form;
const { Option } = Select;
const inlineFormItemLayout = {
  labelCol: {
    sm: {
      span: 8,
    },
  },
  wrapperCol: {
    sm: {
      span: 16,
    },
  },
};

class DetailForm extends React.Component {
  get item() {
    const { propsAPI } = this.props;
    return propsAPI.getSelected()[0];
  }

  handleFieldChange = values => {
    const { propsAPI } = this.props;
    const { getSelected, executeCommand, update } = propsAPI;
    setTimeout(() => {
      const item = getSelected()[0];

      if (!item) {
        return;
      }

      executeCommand(() => {
        update(item, { ...values });
      });
    }, 0);
  };

  handleInputBlur = type => e => {
    e.preventDefault();
    this.handleFieldChange({
      [type]: e.currentTarget.value,
    });
  };

  handleSelect = type => e => {
    this.handleFieldChange({
      [type]: e,
    });
  }

  renderNodeDetail = () => {
    const { label, RejectionType, Roles } = this.item.getModel();
    return (
      <Form
        initialValues={{
          label,
          RejectionType,
          Roles
        }}
      >
        <Item label="Label" name="label" {...inlineFormItemLayout}>
          <Input onBlur={this.handleInputBlur('label')} />
        </Item>
        <Item label="驳回类型" name="RejectionType" {...inlineFormItemLayout}>
          <ComBoBox onChange={this.handleSelect('RejectionType')} />
        </Item>
        <Item label="指定角色" name="Roles" {...inlineFormItemLayout}>
          <ComboGrid
            mode="multiple"
            api="/api/SmRole/GetPageList"
            itemkey="ID"
            itemvalue="RoleName"
            onChange={this.handleSelect('Roles')}
          />
        </Item>
      </Form>
    );
  };

  renderEdgeDetail = () => {
    const { label = '', shape = 'flow-smooth', ConditionField, Condition, ConditionValue } = this.item.getModel();
    return (
      <Form
        initialValues={{
          label,
          shape,
          ConditionField,
          Condition,
          ConditionValue
        }}
      >
        <Item label="Label" name="label" {...inlineFormItemLayout}>
          <Input onBlur={this.handleInputBlur('label')} />
        </Item>
        <Item label="过滤条件" name="ConditionField" {...inlineFormItemLayout}>
          <Input onBlur={this.handleInputBlur('ConditionField')} />
        </Item>
        <Item label="条件" name="Condition" {...inlineFormItemLayout}>
          <Select
            onChange={value =>
              this.handleFieldChange({
                Condition: value,
              })
            }
          >
            <Option value="Boolean">Boolean</Option>
            <Option value="Contains">包含</Option>
            <Option value="Equal">等于</Option>
            <Option value="NoEqual">不等于</Option>
            <Option value="GreaterThan">大于</Option>
            <Option value="GreaterThanOrEqual">大于等于</Option>
            <Option value="LessThan">小于</Option>
            <Option value="LessThanOrEqual">小于等于</Option>
          </Select>
        </Item>
        <Item label="过滤值" name="ConditionValue" {...inlineFormItemLayout}>
          <Input onBlur={this.handleInputBlur('ConditionValue')} />
        </Item>
        <Item label="Shape" name="shape" {...inlineFormItemLayout}>
          <Select
            onChange={value =>
              this.handleFieldChange({
                shape: value,
              })
            }
          >
            <Option value="flow-smooth">Smooth</Option>
            <Option value="flow-polyline">Polyline</Option>
            <Option value="flow-polyline-round">Polyline Round</Option>
          </Select>
        </Item>
      </Form>
    );
  };

  renderGroupDetail = () => {
    const { label = '新建分组' } = this.item.getModel();
    return (
      <Form
        initialValues={{
          label,
        }}
      >
        <Item label="Label" name="label" {...inlineFormItemLayout}>
          <Input onBlur={this.handleInputBlur('label')} />
        </Item>
      </Form>
    );
  };

  render() {
    const { type } = this.props;

    if (!this.item) {
      return null;
    }

    return (
      <Card type="inner" size="small" title={upperFirst(type)} bordered={false}>
        {type === 'node' && this.renderNodeDetail()}
        {type === 'edge' && this.renderEdgeDetail()}
        {type === 'group' && this.renderGroupDetail()}
      </Card>
    );
  }
}

export default withPropsAPI(DetailForm);
