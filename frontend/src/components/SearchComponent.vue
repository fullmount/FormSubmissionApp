<template>
    <div class="form-container">
        <button @click="goBack" class="back-button">Back</button>
        <h2>Search contact</h2>
        <div class="form-group">
            <input v-model="searchCriteria" type="text" placeholder="Введите критерий поиска" class="search-input" />
        </div>
        <button @click="handleSearch" class="search-button">Search</button>
        
        <div v-if="store.forms.length" class="results">
            <h3>Search results:</h3>
            <ol>
                <li v-for="(submission, index) in store.forms" :key="index" class="fade-in">
                    Full name: <strong> {{ submission.fullName }}</strong>, 
                    From: <strong>{{ submission.country }}, </strong>
                    Date of birth: <strong>{{ submission.dob }}, </strong>
                    Gender: <strong>{{ submission.gender }} </strong>
                </li>
            </ol>
        </div>
    </div>
</template>
  
  <script setup lang="ts">
  import { ref } from "vue";
  import { useFormStore } from "../store/formModule";
  import { useRouter } from 'vue-router';
  
  const store = useFormStore();
  const searchCriteria = ref("");

  const router = useRouter();
  const goBack = () => router.push('/');

  
  const handleSearch = () => {
    store.fetchForms(searchCriteria.value);
  };
  </script>
  
  <style scoped lang="scss">
  @use "../assets/form";
  </style>
  