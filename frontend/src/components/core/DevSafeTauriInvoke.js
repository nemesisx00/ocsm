import { invoke } from 'tauri/api/tauri'

export const devSafeInvoke = (arg) => {
	//Failsafe in the case that you're only running the frontend with React's dev server
	try {
		invoke(arg)
	} catch(err) {
		console.log('Tauri `invoke` not available')
	}
}
