import './Contracts.css'
import React from 'react'
import FiveDots from '../../FiveDots'

export default class ContractDetails extends React.Component
{
	render()
	{
		let {labels, dots} = this.generateContracts()
		
		return (
			<div className="contractsWrapper">
				<div className="label">Contracts</div>
				<div className="contracts">
					<div className="column">
						{labels}
						<div onClick={() => this.props.newHandler()}>Add New Contract</div>
					</div>
					<div className="column">
						{dots}
					</div>
				</div>
			</div>
		)
	}
	
	generateContracts()
	{
		let out = {
			labels: [],
			dots: []
		}
		
		let clickHandler = this.props.clickHandler
		let dotsHandler = this.props.dotsHandler
		
		this.props.contracts.forEach((contract, i) => {
			out.labels.push(
				(<div key={`label-${i}`} onClick={() => clickHandler(contract)}>{contract.label}</div>)
			)
			out.dots.push(
				(<FiveDots key={`label-${i}`} value={contract.dots} valueChangedHandler={(value) => dotsHandler(contract, value)} />)
			)
		})
		
		return out
	}
}
