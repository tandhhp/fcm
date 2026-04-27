import { defineConfig } from "@umijs/max";
import defaultSettings from "./config/defaultSetting";
import routes from "./config/routes";

export default defineConfig({
  antd: {
    theme: {
      token: {
        colorPrimary: `#${process.env.PRIMARY_COLOR}`,
        fontFamily: '"Quicksand", sans-serif',
        fontOpticalSizing: 'auto',
        fontWeight: 500
      }
    }
  },
  access: {},
  model: {},
  initialState: {},
  request: {},
  layout: {
    ...defaultSettings,
  },
  locale: {
    default: "vi-VN",
    baseSeparator: "-",
    antd: true,
  },
  history: {
    type: "hash",
  },
  routes,
  npmClient: "yarn",
  tailwindcss: {},
  hash: true,
  mako: {},
  esbuildMinifyIIFE: true,
  define: {
    'process.env': {
      API_URL: process.env.API_URL,
      COMPANY_NAME: process.env.COMPANY_NAME,
      PRIMARY_COLOR: process.env.PRIMARY_COLOR
    }
  }
});