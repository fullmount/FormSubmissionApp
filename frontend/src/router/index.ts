import { createRouter, createWebHashHistory } from 'vue-router';
import SearchView from '../views/SearchView.vue';
import HomeView from '../views/Homeview.vue';

const routes = [
  { path: '/', component: HomeView },
  { path: '/search', component: SearchView }
];

const router = createRouter({
  history: createWebHashHistory(),
  routes
});

export default router;