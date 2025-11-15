import { QuestionCircleOutlined } from '@ant-design/icons';
import { SelectLang as UmiSelectLang } from '@umijs/max';
import { Button } from 'antd';

export type SiderTheme = 'light' | 'dark';

export const SelectLang = () => {
  return (
    <UmiSelectLang />
  );
};

export const Question = () => {
  return (
    <Button type='text' icon={<QuestionCircleOutlined />} />
  );
};