import { defineStore } from 'pinia';
import apiService from '../api/ApiService';
import type { FormData } from '../types/formTypes';

export const useFormStore = defineStore('form', {
  state: () => ({ forms: [] as FormData[] }),
  actions: {
    async fetchForms(searchCriteria?: string) {
      this.forms = await apiService.getSubmissions(searchCriteria);
    },
    async submitForm(data: FormData) {
      return await apiService.submitForm(data);
    }
  }
});