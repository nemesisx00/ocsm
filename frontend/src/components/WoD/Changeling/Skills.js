import React from 'react'
import Tabs from '../../core/Tabs'
import './Skills.css'

export default class Skills extends React.Component
{
	constructor(props)
	{
		super(props)
		
		this.state = {
			mental: {
				academics: 0,
				computer: 0,
				crafts: 0,
				investigation: 0,
				medicine: 0,
				occult: 0,
				politics: 0,
				science: 0
			},
			physical: {
				athletics: 0,
				brawl: 0,
				drive: 0,
				firearms: 0,
				larceny: 0,
				stealth: 0,
				survival: 0,
				weaponry: 0
			},
			social: {
				animalKen: 0,
				empathy: 0,
				expression: 0,
				intimidation: 0,
				socialize: 0,
				streetwise: 0,
				subterfuge: 0
			}
		}
	}
	
	render()
	{
		return (
			<Tabs className="skills">
				<div className="mental">
					Mental
				</div>
				<div className="physical">
					Physical
				</div>
				<div className="social">
					Social
				</div>
			</Tabs>
		)
	}
}
