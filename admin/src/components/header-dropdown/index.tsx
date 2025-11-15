import { Dropdown } from 'antd';
import type { DropDownProps } from 'antd/es/dropdown';
import React from 'react';

export type HeaderDropdownProps = {
  placement?:
    | 'bottomLeft'
    | 'bottomRight'
    | 'topLeft'
    | 'topCenter'
    | 'topRight'
    | 'bottomCenter';
} & Omit<DropDownProps, 'overlay'>;

const HeaderDropdown: React.FC<HeaderDropdownProps> = ({ ...restProps }) => {
  return (
    <Dropdown
      getPopupContainer={(target) => target.parentElement || document.body}
      {...restProps}
    />
  );
};

export default HeaderDropdown;
