<template>
  <div class="form-container">
    <button @click="goToSearch" class="animated-button">🔍 Search contact</button>
    <h2>New contact</h2>
    <form @submit.prevent="handleSubmit" >
      <div class="form-group" :class="{ 'error': $v.fullName.$error }">
        <label for="fullName">Full Name</label>
        <input id="fullName" v-model="form.fullName" type="text" />
        <span v-if="$v.fullName.$error" class="error-message">Full Name is required</span>
      </div>

      <div class="form-group" :class="{ 'error': $v.country.$error }">
        <label for="country">Country</label>
        <select id="country" v-model="form.country">
          <option value="">Select a country</option>
          <option v-for="country in countries" :key="country" :value="country">
            {{ country }}
          </option>
        </select>
        <span v-if="$v.country.$error" class="error-message">Country is required</span>
      </div>

      <div class="form-group" :class="{ 'error': $v.dob.$error }">
        <label for="dob">Date of Birth</label>
        <input id="dob" v-model="form.dob" type="date" />
        <span v-if="$v.dob.$error" class="error-message">Valid Date of Birth is required</span>
      </div>

      <div class="form-group" :class="{ 'error': $v.gender.$error }">
        <label>Gender</label>
        <div>
          <label><input type="radio" value="Male" v-model="form.gender" /> Male</label>
          <label><input type="radio" value="Female" v-model="form.gender" /> Female</label>
        </div>
        <span v-if="$v.gender.$error" class="error-message">You must specify your gender</span>
      </div>

      <div class="form-group" :class="{ 'error': $v.agreeToTerms.$error }">
        <label>
          <input type="checkbox" v-model="form.agreeToTerms" /> I agree to the terms
        </label>
        <span v-if="$v.agreeToTerms.$error" class="error-message">You must agree to the terms</span>
      </div>

      <button type="submit" class="submit-button" :disabled="loading">
          {{ loading ? "Processing..." : "Create contact" }}
        </button>
    </form>
  </div>
</template>
  
<script setup lang="ts">
import { ref } from 'vue';
import { useVuelidate } from '@vuelidate/core';
import { required, minLength, sameAs } from '@vuelidate/validators';
import { useFormStore } from '../store/formModule';
import { useRouter } from 'vue-router';


const form = ref({
  fullName: '',
  country: '',
  dob: '',
  gender: '',
  agreeToTerms: false
});

const countries = ['USA', 'Canada', 'UK', 'Germany', 'France'];

const validations = {
  fullName: { required, minLength: minLength(3) },
  country: { required },
  dob: { required },
  gender: { required },
  agreeToTerms: { required, sameAs: sameAs(true) }
};

const store = useFormStore();

const $v = useVuelidate(validations, form);
const loading = ref(false);

const router = useRouter();
const goToSearch = () => {
  router.push('/search');
};

async function handleSubmit() {
  $v.value.$touch();
  if ($v.value.$invalid) return;

  loading.value = true;
  await store.submitForm(form.value);
  loading.value = false;

  form.value = { fullName: "", country: "", dob: "", gender: "", agreeToTerms: false };
  $v.value.$reset();
};
</script>
  
<style scoped lang="scss">
@use "../assets/form";
</style>
