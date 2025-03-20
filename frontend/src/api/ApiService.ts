import axios from "axios";
import type { FormData } from '../types/formTypes';

const apiClient = axios.create({
  baseURL: "http://localhost:5000/api/forms",
  headers: {
    "Content-Type": "application/json",
  },
});

const apiService = {
  async submitForm(data: FormData) {
    try {
      const response = await apiClient.post("/submit", data);
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        throw new Error(error.response?.data?.message || "API request failed");
      }
      throw new Error("An unknown error occurred");
    }
  },

  async getSubmissions(searchCriteria?: string) {
    try {
      const response = await apiClient.get<FormData[]>("/getSubmissions", {
        params: { searchCriteria },
      });
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        throw new Error(error.response?.data?.message || "API request failed");
      }
      throw new Error("An unknown error occurred");
    }
  },
};

export { apiClient }; 
export default apiService;