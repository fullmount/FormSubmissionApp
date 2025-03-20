import { describe, it, expect, beforeEach } from 'vitest';
import { mount, VueWrapper } from '@vue/test-utils';
import { createTestingPinia } from '@pinia/testing';
import FormComponent from '../../src/components/FormComponent.vue';


describe('FormComponent', () => {
  let wrapper: VueWrapper;

  beforeEach(() => { 
    wrapper = mount(FormComponent, {
      global: {
        plugins: [createTestingPinia({ stubActions: false })],
      },
    });
  });

  it('rendering component', () => {
    expect(wrapper.find('form').exists()).toBe(true);
  });

  it('check validation before submit', async () => {
    await wrapper.find('form').trigger('submit.prevent');
    expect(wrapper.find('.error-message').exists()).toBe(true);
  });
  
});
