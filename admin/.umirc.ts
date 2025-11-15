import { defineConfig } from "@umijs/max";
import defaultSettings from "./config/defaultSetting";
import routes from "./config/routes";

export default defineConfig({
  antd: {
    theme: {
      token: {
        colorPrimary: '#ca8a04',
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
  esbuildMinifyIIFE: true,
  tailwindcss: {},
  hash: true,
  mako: {}
});