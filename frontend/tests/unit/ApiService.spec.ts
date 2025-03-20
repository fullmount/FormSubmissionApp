import { describe, it, expect, afterEach } from 'vitest';
import MockAdapter from 'axios-mock-adapter';
import apiService, { apiClient } from "../../src/api/ApiService";

const mock = new MockAdapter(apiClient);

describe('apiService', () => {
    
    afterEach(() => {
        mock.reset();
    });

    it("should submit a form", async () => {
        mock.onPost("/submit").reply(200, 2);
    
        const response = await apiService.submitForm({ fullName: "Jim Carter" });
    
        expect(response).toBeGreaterThan(0);
        expect(mock.history.post.length).toBe(1);
    });

    it("should retrieve all forms", async () => {
        const mockResponse = [
          { id: 1, fullName: 'Jack Black', country: 'USA' },
          { id: 2, fullName: 'Steve Smith', country: 'Canada' }
        ];
        mock.onGet("/getSubmissions").reply(200, mockResponse);
        const response = await apiService.getSubmissions();
        expect(response).toEqual(mockResponse);
        expect(mock.history.get.length).toBe(1);
    });

    it("should search forms", async () => {
        const mockData = [{ fullName: "Jim Carter" }];
        mock.onGet("/getSubmissions", { params: { searchCriteria: "Jim" } }).reply(200, mockData);

        const response = await apiService.getSubmissions("Jim");

        expect(response).toEqual(expect.arrayContaining([ 
            expect.objectContaining(
              {
                fullName: 'Jim Carter',
              } 
            )
          ]))
        expect(mock.history.get.length).toBe(1);
    });

    it("should handle API errors", async () => {
        mock.onPost("/submit").reply(500);

        await expect(apiService.submitForm({ fullName: "Joe Black" })).rejects.toThrow("API request failed");
    });
});