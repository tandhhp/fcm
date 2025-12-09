import { request } from '@umijs/max';

export interface ChatMessage {
  role: 'user' | 'assistant';
  content: string;
}

export async function apiAIChatCompletion(messages: ChatMessage[]) {
  return request('ai/chat', {
    method: 'POST',
    data: { messages },
  });
}
